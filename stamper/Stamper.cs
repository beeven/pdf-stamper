using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Startup
{
    private Stamper stamper;

    public string pfxFile = @"/myapp/certificates/ca.pfx";
    public string pfxPassword = "123456";
    public string outFileSuffix = "signed";
    public string stampImage = @"/myapp/stamp.png";
    public Startup()
    {
        Pkcs12Store pkcs12 = new Pkcs12Store(new FileStream(pfxFile, FileMode.Open, FileAccess.Read), pfxPassword.ToArray());
        string keyAlias = null;

        foreach (string name in pkcs12.Aliases)
        {
            if (pkcs12.IsKeyEntry(name))
            {
                keyAlias = name;
                break;
            }
        }

        AsymmetricKeyParameter key = pkcs12.GetKey(keyAlias).Key;
        X509CertificateEntry[] ce = pkcs12.GetCertificateChain(keyAlias);
        List<X509Certificate> chain = new List<X509Certificate>(ce.Length);
        foreach (var c in ce)
        {
            chain.Add(c.Certificate);
        }

        stamper = new Stamper()
        {
            CertChain = chain,
            PrivateKey = key,
            Stamp = iTextSharp.text.Image.GetInstance(stampImage)
        };
    }
    public async Task<object> Invoke(dynamic input)
    {
        string filename = (string)input;
        string outFilename = String.Format("{0}-{1}.pdf", filename.Substring(0, filename.LastIndexOf('.')), outFileSuffix);

        return await Task.Run(() =>
        {
            stamper.SignPdf(filename, outFilename);
//            File.Delete(filename);
//            File.Move(outFilename, filename);
            Console.WriteLine("Done signing");
            return outFilename;
        });
    }
}
class Stamper
{


    public AsymmetricKeyParameter PrivateKey { get; set; }
    public List<X509Certificate> CertChain { get; set; }
    public iTextSharp.text.Image Stamp { get; set; }


    public void SignPdf(string filename, string outFilename)
    {

        PdfReader reader = new PdfReader(filename);
        PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(outFilename, FileMode.Create, FileAccess.Write), '\0', null, true);

        PdfSignatureAppearance sap = st.SignatureAppearance;
        sap.Reason = "税单完整性";
        sap.Location = "广州海关";

        sap.SetVisibleSignature(new iTextSharp.text.Rectangle(450,100,650,200),1,"sig");

        sap.ImageScale = 1;
        sap.Layer2Text = "此文档由广州海关签名";
        sap.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
        sap.SignatureGraphic = Stamp;

        IExternalSignature signature = new PrivateKeySignature(PrivateKey, "SHA-256");
        MakeSignature.SignDetached(sap, signature, CertChain, null, null, null, 0, CryptoStandard.CMS);
    }
}
