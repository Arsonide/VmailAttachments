# Vmail Attachments

## What is it?

This plugin does nothing on its own, it exposes helper methods for other mods to use that allow them to attach arbitrary documents to vmails. The player can print these documents separately from the message body.

## Installation

You should not need to manually install this mod, if it is set up as a dependency of another mod, it should install automatically, but for completion sake here is how to install this mod on its own.

### r2modman or Thunderstore Mod Manager installation

If you are using [r2modman](https://thunderstore.io/c/shadows-of-doubt/p/ebkr/r2modman/) or [Thunderstore Mod Manager](https://www.overwolf.com/oneapp/Thunderstore-Thunderstore_Mod_Manager) for installation, simply download the mod from the Online tab.

### Manual installation

Follow these steps:

1. Download BepInEx from the official repository.
2. Extract the downloaded files into the same folder as the "Shadows of Doubt.exe" executable.
3. Launch the game, load the main menu, and then exit the game.
4. Download the latest version of the plugin from the Releases page. Unzip the files and place them in corresponding directories within "Shadows of Doubt\BepInEx...". Also, download the [SOD.Common](https://thunderstore.io/c/shadows-of-doubt/p/Venomaus/SODCommon/) dependency.
5. Start the game.

## Usage and features

### Configuration

There are no configuration settings. The mod has its own save store where it saves the vmail attachments as JSON.

### Modders

There are two static methods in VmailAttachmentsPlugin that you may use, once you reference this plugin:
* VmailAttachmentsPlugin.AddAttachment: This allows you to associate an attachment with a vmail ID. This association is separate from the vmail itself, which you will need to send. (Helper methods may be added in the future.) You will need an InteractablePreset for the document and human IDs for who the document references.

* VmailAttachmentsPlugin.TryGetAttachment: This allows you to check if a vmail has an attachment. You should not need this though, once an attachment is added to a vmail the mod handles all UI for printing the document at that point.

## License

All code in this repo is distributed under the MIT License. Feel free to use, modify, and distribute as needed.
