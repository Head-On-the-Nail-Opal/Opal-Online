# Opal-Online

## Getting Started/Cloning the repository:
<b> Requirements: </b> 
* Git
* Git Large File Storage (will install below)
* Terminal (Recommend GitBash for easier navigation since Command Prompt is a pain-- download GitBash [here](https://gitforwindows.org/) as part of Git for Windows) 


In bash terminal, enter the following commands one at a time 
```
git lfs install 
```

*Make sure that you are cloning the repository where you want it in the file system* 

<ins> Note: some files have really long paths-- may not be able to correctly clone in directories that are within multiple subfolders already <ins> 


```
git clone git@github.com:Head-On-the-Nail-Opal/Opal-Online.git
```


## Troubleshooting: 
<b> Error: </b> Path too long
<b> Reasoning: </b> There are some files with really long path names, and if you are trying to clone the repo within other folders in the file system, the overall path may be too long. 
<b> Fix: </b>

* Current option: delete the repo as it currently exists and try again in a different place in the file structure 

### Deleting Git Repo: 
* To start, delete the .git file in the repo either via command line or file system 
``` 
rm <file-name-here-don't-include-angle-brackets>
```

* Using command line, navigate to the place the git repo exists 
* Delete the incorrectly cloned folders and contents: 

*Note: be careful when using this command that you are deleting the correct folders-- whatever you delete with this command cannot be recovered 
```
rm -r Opal-Online
```
