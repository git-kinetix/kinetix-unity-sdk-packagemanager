# Kinetix Unity SDK Package Manager

This is a Unity custom package that give you access to Kinetix Custom Packages modules as Core functionnalities and UI.

## Quick Start 

### Requirements
- Unity Version 2020.3 or higher
- Git needs to be installed to fetch the Unity packages through the Unity Package Manager.

**1.** Open your Unity project and open the Unity Package Manager Window on "Window > Package Manager".

<img width="738" alt="image" src="https://user-images.githubusercontent.com/111740346/232071162-21838139-6dac-4404-a12f-27c9b0c2cee6.png">


**2.** In the **Package Manager** window click on the " **+** " icon in the top left corner and select **Add Package From Git URL**.

<img width="500" alt="image" src="https://user-images.githubusercontent.com/111740346/232071810-4ba400bc-d566-40cb-9aae-e76efcfa9a26.png">


**3.** Paste in this url 


```html
https://github.com/git-kinetix/kinetix-unity-sdk-packagemanager.git
```

<img width="503" alt="image" src="https://user-images.githubusercontent.com/111740346/232072205-98d1efd0-fc87-47d0-b5fe-ac852bffb56c.png">


**4.** Click add and wait for the import process to finish.


**5.** Install dependencies if you don't already have those packages by clicking on the " **+** " icon on the package manager with the Git URL and paste :

```html
com.unity.nuget.newtonsoft-json
```
and
```html
com.unity.inputsystem
```

While adding the "com.unity.input.system" package, you may have a warning popup, select **Yes**

<img width="650" alt="image" src="https://user-images.githubusercontent.com/111740346/232103840-c84c5bd4-9211-4b51-9581-58880b207a24.png">


**6.** As our UI use new Input system, verify in "**Edit/Project Settings...**" and in the "**Player**" that your "**Active Input Handling***" is set 
to "**Input System Package (New)**" or "**Both**"

<img width="837" alt="image" src="https://user-images.githubusercontent.com/111740346/232101359-93d61e48-fca9-4105-8635-b5ddfc698e63.png">


**7.** Import TMP Essential ressources by clicking on "**Window/TextMeshPro/Import TMP Essential Ressources**"

<img width="787" alt="image" src="https://user-images.githubusercontent.com/111740346/232073524-53d6343d-b754-4d77-ab08-ae26024f99fe.png">


**8.** Open the Kinetix Package Manager Window 

<img width="525" alt="image" src="https://user-images.githubusercontent.com/111740346/232073991-2abc2c5f-1c8c-477a-8b1b-45944730cb7d.png">


**9.** Click on "Install Core Bundle" for the Web2 Bundle or Web3 Bundle

<i>This process will install the Core, UICommon and UIEmoteWheel Packages</i>

<img width="460" alt="image" src="https://user-images.githubusercontent.com/111740346/232074178-0c94c3e7-bee4-46c1-8296-76c208d3aaca.png">

On Unity 2020.x versions specifically, you may have an issue starting with :

`Error: Could not load signature of Kinetix.Internal.Retargeting.Utils.CustomTransformLogger:DrawSkeleton due to: Could not load file or assembly 'Kinetix.Internal.Utils`

If this happens, you can reimport the Kinetix Core Package by right clicking on "Kinetix Core" in "Packages" and select "Reimport".

<img width="395" alt="image" src="https://user-images.githubusercontent.com/111740346/232483957-e8c9963e-ab5b-4020-8606-36ac5d59edbd.png">


**10.** Verify the packages are installed in the Unity Package Manager, click on the package "Kinetix UI Emote Wheel" and import the samples

<img width="965" alt="image" src="https://user-images.githubusercontent.com/111740346/232077026-55ac9ca8-03e3-4004-b48b-013bafbfc273.png">


**11.** Get your VirtualWorldKey through the next link

=> https://docs.kinetix.tech/gs/unity/download-and-set-up/get-your-virtualworldkey

**12.** Open the "Basic Scene" sample from "Basic Demo" sample folder

<img width="779" alt="image" src="https://user-images.githubusercontent.com/111740346/232077800-ea652870-72a3-4a6f-866a-2a5b962b0723.png">

**13.** Click on "SampleScript" and put your VirtualWorldKey in the Inspector.

<img width="820" alt="image" src="https://github.com/git-kinetix/kinetix-unity-sdk-packagemanager/assets/111740346/30c4f7e8-6ad2-4093-bdee-ddb6fb9d1bad">

**14.** Play and enjoy the emotes 

<img width="872" alt="image" src="https://user-images.githubusercontent.com/111740346/232078058-e28b48b2-9235-47c9-9b64-b7092fae708f.png">
