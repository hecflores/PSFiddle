Param(
	[String] $SourceAppConfig,
	[String] $TransformAppConfig
)
function XmlDocTransform($xmlpath, $xdtpath)
{

      if (!($xmlpath) -or !(Test-Path -path ($xmlpath) -PathType Leaf)) {
         Write-Host "Base file not found. $xmlpath";
		 return
      }

      if (!($xdtpath) -or !(Test-Path -path ($xdtpath) -PathType Leaf)) {
         Write-Host "Transform file not found. $xdtpath";
		 return

      }

      Add-Type -LiteralPath "$PSScriptRoot\..\..\AppSettingsTransform\Microsoft.Web.XmlTransform.dll"

      $xmldoc = New-Object   Microsoft.Web.XmlTransform.XmlTransformableDocument;
      $xmldoc.PreserveWhitespace = $true
      $xmldoc.Load($xmlpath);

      $transf = New-Object Microsoft.Web.XmlTransform.XmlTransformation($xdtpath);
      if ($transf.Apply($xmldoc) -eq $false)
      {
          throw "Transformation failed."
      }
      $xmldoc.Save($xmlpath);

      Write-Host "Transformation succeeded" -ForegroundColor Green
  }

  XmlDocTransform $SourceAppConfig $TransformAppConfig