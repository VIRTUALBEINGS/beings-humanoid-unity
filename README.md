# Humanoid Being - Unity

## VIRTUAL BEINGS HUB Discord server
[Feel free to join our developer's Discord server !](https://discord.gg/raHeeqbh)

## Requirements
- Unity Version 2021.3 or higher
- Universal Render Pipeline Unity project
- [Git](https://git-scm.com/)
- [VIRTUAL BEINGS Tech Unity package](https://github.com/VIRTUALBEINGS/virtualbeingstech-unity)

## How to import Humanoid Being package into your Unity project
- Open Package Manager windows in Unity : `Window > Package Manager`.

![image](https://user-images.githubusercontent.com/128504226/226867218-df18b6ca-9977-4e09-a599-7d5e9dd03946.png)

- Click on the `+` icon and select `Add package from git URL...`.

![image](https://user-images.githubusercontent.com/128504226/226867580-119c6f85-2921-4247-8805-17250946e1c6.png)

- Paste this link : `https://github.com/VIRTUALBEINGS/beings-humanoid-unity.git`

![image](https://user-images.githubusercontent.com/128504226/226867806-e32be7ad-425a-4328-8223-2035f3af02b6.png)

- Click `Add` and wait until the import is finished

## Quickstart
**1.** Open `Virtual Beings > Settings` and set up your key. If you don't have one, you can use "Demo".

![image](https://user-images.githubusercontent.com/128504226/226871467-41f600a7-0361-4433-99b9-aba4cb27ae9e.png)

![image](https://user-images.githubusercontent.com/128504226/226871568-66b4fea6-1975-4ac2-8a4a-0f83f6d1b7ac.png)

**2.** Download the QuickStart sample from the package manager.

![image](https://user-images.githubusercontent.com/128504226/226871364-deb7a839-a90c-45a5-890a-ef5352b556ef.png)

**3.** Open the scene from the sample.

**4.** Select the BeingsInstallerSettings in the sample folder, and fill the `Virtual Being Settings` field with the corresponding asset in your project folder.

![image](https://user-images.githubusercontent.com/128504226/226872078-81b72b8f-ea6c-477e-ad77-5633ad8d3eb5.png)

![image](https://user-images.githubusercontent.com/128504226/226872159-1aca6e26-5623-43a3-80b4-7e7c22df5b7e.png)

**5.** You can now start the scene

![image](https://user-images.githubusercontent.com/128504226/226872944-039cac5d-4541-4375-94bb-a263bd5006c8.png)

## (Optional) Import your own ReadyPlayerMe Being

### Requirements

- [RPM Unity Core](https://github.com/readyplayerme/rpm-unity-sdk-core)

### Create your Being

**1.** Import ReadyPlayerMe avatar loading sample

![image](https://user-images.githubusercontent.com/128504226/226882570-8376429e-d7f9-4242-b0b3-ac4a0bf88250.png)

**2.** Open Avatar Loader windows :  `Ready Player Me > Avatar Loader`

![image](https://user-images.githubusercontent.com/128504226/226882835-3716e620-c142-444c-ab90-99bb35b0cee1.png)

**3.** Import your RPM avatar into the current scene

![image](https://user-images.githubusercontent.com/128504226/226882932-ac7a878a-a516-45a6-a039-c142e7dfb2a5.png)

**4.** Open your new avatar prefab, and add `PostProcessAnimationHumanoid.cs` script at the root prefab

![image](https://user-images.githubusercontent.com/128504226/226883592-c41b141d-9e60-48e6-95fa-ae2065c0ae1a.png)

**5.** On the `PostProcessAnimationHumanoid` component, configure your avatar as a being by :
- Filling the `Being Shared Settings` field
- Click on `1 - Create Being`
- Click on `2 - Create Colliders`
- Click on `3 - Create Drivers`

![image](https://user-images.githubusercontent.com/128504226/226884183-7f92e87a-f107-4b9e-9ad0-a6dbc9d6f5c9.png)


Your new Being is now ready. You can use it in the QuickStart scene by replacing the being prefab on the `Being Installer Settings Sample`.

![image](https://user-images.githubusercontent.com/128504226/226884446-a73ef2cf-e47c-4994-a3d2-20ae4c9fa475.png)
