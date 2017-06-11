# Azure RMS test app
## Requirements
- Installed and configured [Azure RMS Connector](https://docs.microsoft.com/en-us/information-protection/deploy-use/install-configure-rms-connector) on a server
- Installed [AD RMS Client 2.1](https://www.microsoft.com/en-us/download/details.aspx?id=38397)
- Applied registry settings that will redirect all RMS communictaion to RMS Connector. This registry setttings should by applied on computer/server that is used for running the AzureRMSTestApp

# 
The application implements three methods:
- List all available RMS templates ([IpcGetTemplateList](http://msdn.microsoft.com/en-us/library/windows/desktop/hh535267(v=vs.85).aspx))
- Decrypt encrypted file ([IpcfDecryptFile](https://msdn.microsoft.com/en-us/library/windows/desktop/dn133058(v=vs.85).aspx))
- Encrypt file ([IpcfEncryptFile](https://msdn.microsoft.com/en-us/library/windows/desktop/dn133059(v=vs.85).aspx))

Unfortunatelly method that is used for encrypting file does not work and ends with an error **IPCERROR_NEEDS_ONLINE** even if offline flag is set to false.
```C#
SafeFileApiNativeMethods.IpcfEncryptFile(
    inputFile: filePath,
    templateId: template.TemplateId,
    flags: SafeFileApiNativeMethods.EncryptFlags.IPCF_EF_FLAG_DEFAULT,
    suppressUI: true,
    offline: false,
    hasUserConsent: true,
    parentForm: null,
    symmKey: null,
    outputDirectory: null);
```