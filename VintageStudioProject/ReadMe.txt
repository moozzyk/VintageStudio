1. Returning null from ProjectNode.GetReferenceContainer() method causes NullReferenceException.
   Because we don't want to show "References" node we need to return null from Visual65XXProjectNode.CreateReferenceContainerNode(). Unfortunately this causes a NullReferenceException in the 
   ProjectConfig.RefereshReferences() method. Since there is no way to override the behavior of this method (the method is private) the bug needs to be fixed directly in the MPFProj sources by
   checking the return value and doing nothing if it is null instead of blindly dereferencing the value returned from ProjectNode.CreateReferenceContainerNode() method (or any override of this method)

2. Adding a code file to a VintageStudio project. 
   When adding an existing text file to a project in VS the default encoding will be UTF-16. 64tass (and potentially other compilers) can read only ASCII files. To do this in VS you have to do the following:

3. Build action for code files

4. Warning: Microsoft.VsSDK.targets(889,5): warning : File 'AsmCodeFile.ico' referenced from VSTemplate does not exist.
   This is a bug in VS. The warning can be ignored http://connect.microsoft.com/VisualStudio/feedback/details/552332/warning-file-bitmap-bmp-referenced-from-vstemplate-does-not-exist