![VIRTUAL BEINGS banner](Resources~/GitHub%20readme%20banner.jpg)

# VIRTUAL BEINGS Unity Humanoid Being

<img src="Resources~/discord_icon.png" width=16 /> [**VIRTUAL BEINGS Hub on Discord**](https://discord.gg/raHeeqbh)

## Requirements
- Unity Version 2021.3 or higher
- Universal Render Pipeline Unity project
- [Git](https://git-scm.com/)
- [VIRTUAL BEINGS Tech Unity package](https://github.com/VIRTUALBEINGS/virtualbeingstech-unity)

## How to import Humanoid Being package into your Unity project
- Open Package Manager windows in Unity : `Window > Package Manager`.

![image](Resources~/readme_screenshot1.png)

- Click on the `+` icon and select `Add package from git URL...`.

![image](Resources~/readme_screenshot2.png)

- Paste this link : `https://github.com/VIRTUALBEINGS/beings-humanoid-unity.git`

![image](Resources~/readme_screenshot3.png)

- Click `Add` and wait until the import is finished

## Quickstart
**1.** Open `Virtual Beings > Settings` and set up your key. If you don't have one, you can use "Demo".

![image](Resources~/readme_screenshot4.png)

![image](Resources~/readme_screenshot5.png)

**2.** Download the QuickStart sample from the package manager.

![image](Resources~/readme_screenshot6.png)

**3.** Open the scene from the sample.

**4.** Select the BeingsInstallerSettings in the sample folder, and fill the `Virtual Being Settings` field with the corresponding asset in your project folder.

![image](Resources~/readme_screenshot7.png)

![image](Resources~/readme_screenshot8.png)

**5.** You can now start the scene

![image](Resources~/readme_screenshot9.png)

## (Optional) Import your own ReadyPlayerMe Being

### Requirements

- [RPM Unity Core](https://github.com/readyplayerme/rpm-unity-sdk-core)

### Create your Being

**1.** Import ReadyPlayerMe avatar loading sample

![image](Resources~/readme_screenshot10.png)

**2.** Open Avatar Loader windows :  `Ready Player Me > Avatar Loader`

![image](Resources~/readme_screenshot11.png)

**3.** Import your RPM avatar into the current scene

![image](Resources~/readme_screenshot12.png)

**4.** Open your new avatar prefab, and add `PostProcessAnimationHumanoid.cs` script at the root prefab

![image](Resources~/readme_screenshot13.png)

**5.** On the `PostProcessAnimationHumanoid` component, configure your avatar as a being by :
- Filling the `Being Shared Settings` field
- Click on `1 - Create Being`
- Click on `2 - Create Colliders`
- Click on `3 - Create Drivers`

![image](Resources~/readme_screenshot14.png)


Your new Being is now ready. You can use it in the QuickStart scene by replacing the being prefab on the `Being Installer Settings Sample`.

![image](Resources~/readme_screenshot15.png)
