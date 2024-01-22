using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CENTIS.UnityFileExplorer
{
    public class TreeViewNode<T>
    {
        public UINode UIInstance { get; private set; }
        public UINode UIButton { get; private set; }

        //private Label mLabel = null;
        //private Button mButton = null;
        private TreeNode<string> mNodeRef = null;
        private ExplorerManager mMgrRef = null;
        private TreeViewNode<T> mParentRef = null;
        private readonly List<TreeViewNode<T>> mChildren = new List<TreeViewNode<T>>();
        private bool mUnfold = false;
        private int mChildIndex = -1;
        private int mRowIndex = 0;

        // private static TreeViewNode<T> mActiveNodeRef = null;

        private const int mBtnSize = 25;

        public TreeViewNode(TreeNode<string> node, ExplorerManager manager)
        {
            InititializeNode(node, manager);
            int lastRowIndex = mRowIndex;
            Uncover(ref lastRowIndex);
        }

        private TreeViewNode(TreeNode<string> node, ExplorerManager manager, TreeViewNode<T> parent, int index)
        {
            mParentRef = parent;
            mChildIndex = index;
            InititializeNode(node, manager);
        }
        private void InititializeNode(TreeNode<string> node, ExplorerManager manager)
        {
            mNodeRef = node;
            mMgrRef = manager;

            UIInstance = GameObject.Instantiate(
                manager.ExplorerConfiguration.FolderPrefab,
                manager.FileContainer.transform);
            UIInstance.gameObject.name = ToString();
            UIInstance.gameObject.GetComponent<Image>().color = Color.cyan;
            UIInstance.gameObject.SetActive(false);
            //UIInstance.OnSelected += () => manager.SelectNode(this);
            //UIInstance.OnDeselected += () => manager.DeselectNode(this);
            UIInstance.OnActivated += () => manager.ActivateNode(node, UIInstance);
            UIInstance.OnActivated += () => Button_Click_2();
            UIInstance.Initiate(node.Name); // Name setzen

            UIButton = GameObject.Instantiate(
                manager.ExplorerConfiguration.FilePrefab,
                manager.FileContainer.transform);

            UIButton.gameObject.name = ToString();
            UIButton.gameObject.GetComponent<Image>().color = Color.cyan;
            UIButton.gameObject.SetActive(false);
            //UIInstance.OnSelected += () => manager.SelectNode(this);
            //UIInstance.OnDeselected += () => manager.DeselectNode(this);
            UIButton.OnActivated += () => manager.ActivateNode(node, UIButton);
            UIButton.OnActivated += () => Button_Click();
            UIButton.Initiate("+"); // Name setzen
            //mLabel = new Label();
            //mLabel.AutoSize = true;
            //mLabel.Text = node.Name;
            //mLabel.Name = node.Name;
            //mLabel.Height = mBtnSize;
            //mLabel.Visible = false;
            //mLabel.Click += Label_Click;
            //form.Controls.Add(mLabel);

            if (!node.Atomic)
            {
                //mButton = new Button();
                //mButton.Size = new System.Drawing.Size(mBtnSize, mBtnSize);
                //mButton.Name = node.Name;
                //SetText(node);
                UIInstance.Initiate(node.Name + " ->");
                //UIInstance.gameObject.name += mUnfold ? " -" : " +";
                //mButton.Enabled = !(node.Children.Count == 0);
                //mButton.Visible = false;
                //mButton.Click += Button_Click;
                //form.Controls.Add(mButton);

                for (int iChild = 0; iChild < node.Children.Count; ++iChild)
                    mChildren.Add(new TreeViewNode<T>(node.Children[iChild], manager, this, iChild));
            }
        }

        //private void Label_Click(System.Object sender, System.EventArgs e)
        //{
        //    mLabel.BackColor = Color.LightBlue;
        //    if (mActiveNodeRef != null)
        //        mActiveNodeRef.mLabel.BackColor = mFormRef.BackColor;
        //    mActiveNodeRef = this;
        //}

        private void Button_Click()
        {
            // toggle visibility of children
            mUnfold = !mUnfold;
            SetText();
            //UIInstance.name = mUnfold ? "-" : "+";
            
            //SetText();
            //UpdateColor();
            //ChangeColor(Color.cyan);

            if (mChildren.Count == 0) return;
            int lastRowIndex = mRowIndex;
            
            if (mUnfold)
            {
                foreach (var child in mChildren)
                {
                    lastRowIndex++;
                    child.Uncover(ref lastRowIndex);
                }
            }
            else
            {
                foreach (var child in mChildren)
                {
                    child.Cover();
                }
            }

            // adapt location of all nodes below the current one
            UpdateAllNext(lastRowIndex);
            //mFormRef.Refresh();
        }

        private void Button_Click_2()
        {
            UpdateColor();
            ChangeColor(Color.white);
        }

        private void Uncover(ref int lastRowIndex)
        {
            UpdateLocation(lastRowIndex);
            UpdateVisibility(true);
            if (mUnfold)
            {
                foreach (var child in mChildren)
                {
                    lastRowIndex++;
                    child.Uncover(ref lastRowIndex);
                }
            }
        }

        private void Cover()
        {
            UpdateVisibility(false);
            foreach (var child in mChildren)
                child.Cover();
        }

        private void UpdateAllNext(int lastRowIndex)
        {
            if (mParentRef != null)
            {
                mParentRef.UpdateChildren(mChildIndex + 1, ref lastRowIndex);
                mParentRef.UpdateAllNext(lastRowIndex);
            }
        }

        private void UpdateChildren(int childIndexStart, ref int lastRowIndex)
        {
            if (mUnfold)
            {
                for (int iChild = childIndexStart; iChild < mChildren.Count; ++iChild)
                {
                    lastRowIndex++;
                    mChildren[iChild].UpdateLocation(lastRowIndex);
                    mChildren[iChild].UpdateChildren(0, ref lastRowIndex);
                }
            }
        }

        private void UpdateLocation(int rowIndex)
        {
            mRowIndex = rowIndex;

            Vector3 pos = UIInstance.gameObject.transform.position;
            pos.x = 795 + mNodeRef.Depth * mBtnSize;
            pos.y = 670 - rowIndex * 2 * mBtnSize;
            pos.z = 0;

            Vector3 pos_2 = UIButton.gameObject.transform.position;
            pos_2.x = 700 + mNodeRef.Depth * mBtnSize;
            pos_2.y = 670 - rowIndex * 2 * mBtnSize;
            pos_2.z = 0;


            UIInstance.gameObject.transform.position = pos;
            UIButton.gameObject.transform.position = pos_2;
            //    mLabel.Location = new System.Drawing.Point((mNodeRef.Depth + 1) * mBtnSize, rowIndex * mBtnSize);
            Debug.Log(mNodeRef.Depth);
            Debug.Log(UIInstance.GetComponent<RectTransform>().transform.position);
        }

        private void UpdateVisibility(bool visible)
        {
          
            UIInstance.gameObject.SetActive(visible);
            if (!mNodeRef.Atomic) UIButton.gameObject.SetActive(visible);
            //mLabel.Visible = visible;
        }

        private void UpdateColor()
        {
            if (mParentRef != null) mParentRef.UpdateColor();
            UpdateColorChildren();
            ChangeColor(Color.cyan);  
        }

        private void UpdateColorChildren()
        {
            for (int iChild = 0; iChild < mChildren.Count; ++iChild)
            {
                mChildren[iChild].ChangeColor(Color.cyan);
                mChildren[iChild].UpdateColorChildren();
            }
        }

        private void ChangeColor(Color color)
        {
            Image image = UIInstance.gameObject.GetComponent<Image>();
            image.color = color;
        }

        private void SetText()
        {
            //if (!mUnfold && !mNodeRef.Atomic) UIInstance.Initiate(mNodeRef.Name + " ->");
            //else if (!mNodeRef.Atomic) UIInstance.Initiate(mNodeRef.Name + " <-");

            if (!mUnfold) UIButton.Initiate("+");
            else UIButton.Initiate("-");
        }
    }
}