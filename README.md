# MscrmTools.SolutionComponentsDeduplicator
An XrmToolBox tool to identify components present in multiple solutions and clean them

## Disclaimer
This tool is aimed to process Solution components. I was not able to test all component types so some of them might not process correctly. If you detect component types not handled correctly, please create an issue in this repository

## How to use it?
Once connected to your development environment, click on the button **Load** to load solutions and other required information
<img width="686" height="163" alt="image" src="https://github.com/user-attachments/assets/6bd7982d-6d7b-4a6e-87fb-6a81cdb0febb" />

Select at least two solutions and click on the button **Analyze Solutions**
<img width="686" height="163" alt="image" src="https://github.com/user-attachments/assets/89f09680-05cc-413a-88a7-55c6c888463a" />

A list of components that are present in at least two of the solutions you selected is displayed
<img width="1514" height="605" alt="image" src="https://github.com/user-attachments/assets/911f095c-3b27-4f91-ba12-7874407a467b" />

Check the components you want to process, select the solution that should the one to keep the components and click on button **Apply**. The tool will remove the components from the solutions that are not the one selected.
<img width="1898" height="927" alt="image" src="https://github.com/user-attachments/assets/2b0ffdc7-b1ea-459a-af23-1d912eef1c35" />

You can follow the result in the log window at the bottom of the tool.

### Suggestion feature
If components detected are in solutions with different publisher, the button **Check target environment** is visible.
<img width="648" height="207" alt="image" src="https://github.com/user-attachments/assets/0d7a3b59-41ea-482b-ad99-e0f57a588f79" />

This button allows to detect what is the base layer of the checked components in a target environment. This will then suggest to use one of the solution that has the same publisher than the base layer detected (a green checkmark is displayed, or a green box for Table component)
<img width="1482" height="585" alt="image" src="https://github.com/user-attachments/assets/0f2c1792-d698-4945-b45e-7f4c3c668e2f" />
