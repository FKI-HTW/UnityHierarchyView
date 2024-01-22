using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;


namespace CENTIS.UnityFileExplorer
{
    public class ExplorerManager : MonoBehaviour
    {
        #region fields

        public ExplorerConfiguration ExplorerConfiguration { get => _explorerConfiguration; set => _explorerConfiguration = value; }
        [SerializeField] private ExplorerConfiguration _explorerConfiguration;

        public GameObject FileContainer { get => _fileContainer; }
        [SerializeField] private GameObject _fileContainer;

        //private VirtualFolderNode _root; // a virtual folder above the disks
        //private VirtualFolderNode _currentFolder;
        private TreeNode<string> _selectedNode;

        //private readonly List<VirtualFolderNode> _lastVisitedNodes = new(); // for back arrow
        //private readonly List<VirtualFolderNode> _lastReturnedFromNodes = new(); // for forward arrow

        private readonly HashSet<TreeNode<string>> _hashedNodes = new(); // hashset with all node references for O(1) access

        private Action<string> _fileFoundCallback;

        #endregion

        private void Start()
        {
            TreeNode<string> testNode = new TreeNode<string>("Test", "Test", false);
            TreeNode<string> Child1 = testNode.AddChild("Gemuese", "Gemuese", false);
            TreeNode<string> Child11 = Child1.AddChild("Moehren", "Moehren", true);
            TreeNode<string> Child12 = Child1.AddChild("Erbsen", "Erbsen", true);
            TreeNode<string> Child13 = Child1.AddChild("Kohlrabi", "Kohlrabi", true);
            TreeNode<string> Child2 = testNode.AddChild("Fleisch", "Fleisch", false);
            TreeNode<string> Child21 = Child2.AddChild("Rind", "Rind", false);
            TreeNode<string> Child211 = Child21.AddChild("Roulade", "Roulade", true);
            TreeNode<string> Child212 = Child21.AddChild("Steak", "Steak", true);
            TreeNode<string> Child22 = Child2.AddChild("Gefluegel", "Gefluegel", false);
            TreeNode<string> Child221 = Child22.AddChild("Huhn", "Huhn", false);
            TreeNode<string> Child222 = Child22.AddChild("Pute", "Pute", false);
            TreeNode<string> Child2221 = Child222.AddChild("Steak", "Steak", true);
            TreeNode<string> Child2222 = Child222.AddChild("Keule", "Keule", true);
            TreeNode<string> Child3 = testNode.AddChild("Kartoffeln", "Kartoffeln", true);

            ShowTree(testNode); // for testing
        }

        #region public methods

        public void ShowTree(TreeNode<string> node)
        {
            TreeViewNode<string> viewNode = new TreeViewNode<string>(node, this);
            // viewNode.Show(); // for testing
        }

        /* Commented out for testing purposes, do not remove!
         * This is used to navigate to the given startFolder and create 
         * all nodes, that are visited during the navigation.
         * 
        string startFolderPath = Environment.GetFolderPath(startFolder);
        DirectoryInfo startDir = new(startFolderPath);
        VirtualFolderNode startParent = FindParentRecursive(startDir);
        FolderNode startNode = new(this, startDir.GetNodeInformation(), startParent);
        startParent.AddChild(startNode);
        _hashedNodes.Add(startNode);
        NavigateToNode(startNode);
        */

        //public void CancelFindFile()
        //{
        //    _fileFoundCallback = null;
        //    // TODO : close explorer ?
        //}

        //public void SelectNode(TreeNode node)
        //{
        //    if (node == null) return;

        //    _selectedNode = node;
        //}

        //public void DeselectNode(TreeNode node)
        //{
        //    if (_selectedNode == node)
        //        _selectedNode = null;
        //}

        public void ActivateNode(TreeNode<string> node, UINode uiInstance)
        {
            if (node == null) return;

            switch (node.Atomic)
            {
                case false: //FolderNode folderNode
                    //NavigateToNode(folderNode);
                    break;
                case true: //FileNode fileNode
                    //ChooseFile(fileNode)
                  
                    break;
            }
            
        }

        //public void GoBack()
        //{
        //    if (_lastVisitedNodes.Count == 0) return;

        //    VirtualFolderNode targetNode = _lastVisitedNodes[^1];
        //    _lastVisitedNodes.RemoveAt(_lastVisitedNodes.Count - 1);
        //    _currentFolder.NavigateFrom();
        //    targetNode.NavigateTo();
        //    _lastReturnedFromNodes.Add(_currentFolder);
        //    _currentFolder = targetNode;
        //}

        //public void GoForward()
        //{
        //    if (_lastReturnedFromNodes.Count == 0) return;

        //    VirtualFolderNode targetNode = _lastReturnedFromNodes[^1];
        //    _lastReturnedFromNodes.RemoveAt(_lastReturnedFromNodes.Count - 1);
        //    _currentFolder.NavigateFrom();
        //    targetNode.NavigateTo();
        //    _lastVisitedNodes.Add(_currentFolder);
        //    _currentFolder = targetNode;
        //}

        //#endregion

        //#region private methods

        //private void NavigateToNode(FolderNode node)
        //{
        //    _currentFolder.NavigateFrom();
        //    if (!IsFolderLoaded(node))
        //    {
        //        AddDirectories(node);
        //        AddFiles(node);
        //    }
        //    node.NavigateTo();
        //    _lastVisitedNodes.Add(_currentFolder);
        //    _lastReturnedFromNodes.Clear();
        //    _currentFolder = node;
        //}

        //private void ChooseFile(FileNode node)
        //{
        //    _fileFoundCallback?.Invoke(node.ToString());
        //    // TODO : close explorer ?
        //}

        //private bool IsFolderLoaded(VirtualFolderNode folder)
        //{
        //    string folderPath = folder.ToString();
        //    int containedDir = Directory.GetDirectories(folderPath).Length;
        //    int containedFiles = Directory.GetFiles(folderPath).Length;
        //    return folder.Children.Count >= containedDir + containedFiles;
        //}

        //private void AddDirectories(VirtualFolderNode folder)
        //{
        //    string folderPath = folder.ToString();
        //    IEnumerable<DirectoryInfo> containedDir = new DirectoryInfo(folderPath).GetDirectories();
        //    foreach (DirectoryInfo dir in containedDir)
        //    {
        //        FolderNode folderNode = new(this, dir.GetNodeInformation(), folder);
        //        folder.AddChild(folderNode);
        //        _hashedNodes.Add(folderNode);
        //    }
        //}

        //private void AddFiles(VirtualFolderNode folder)
        //{
        //    string folderPath = folder.ToString();
        //    IEnumerable<FileInfo> containedFiles = new DirectoryInfo(folderPath).GetFiles();
        //    foreach (FileInfo file in containedFiles)
        //    {
        //        FileNode fileNode = new(this, file.GetNodeInformation(), folder);
        //        folder.AddChild(fileNode);
        //        _hashedNodes.Add(fileNode);
        //    }
        //}

        //private bool TryFindNode(DirectoryInfo userDir, out TreeNode node)
        //{
        //    return _hashedNodes.TryGetValue(new VirtualFolderNode(this, userDir.GetNodeInformation(), null), out node);
        //}

        //private FolderNode FindParentRecursive(DirectoryInfo userDir)
        //{
        //    if (TryFindNode(userDir.Parent, out TreeNode parent))
        //        return (FolderNode)parent;

        //    FolderNode nextParent = FindParentRecursive(userDir.Parent);
        //    FolderNode newNode = new(this, userDir.Parent.GetNodeInformation(), nextParent);
        //    nextParent.AddChild(newNode);
        //    _hashedNodes.Add(newNode);
        //    return newNode;
        //}

        #endregion

    }
}
