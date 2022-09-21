using System.Drawing;


namespace DigitalStore.WebUI.ExtensionClasses
{
	static public class ImageConvertor
	{
		static public byte[] ConvetrImageToByteArray(IFormFile image)
		{
			try
			{
				byte[] imageBytes = { };

				if (image.Length > 0)
				{
					using (var ms = new MemoryStream())
					{
						image.CopyTo(ms);
						var fileBytes = ms.ToArray();
						imageBytes = fileBytes;
					}
				}
				return imageBytes;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		static public Image ConvetrByteArrayToImage(byte[] array)
		{
			try
			{
				using (var ms = new MemoryStream(array))
				{
					Image i = new Bitmap(ms);
					return i;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		static public string ConvetrByteArrayToString(byte[] array)
		{
			var base64 = Convert.ToBase64String(array);
			return string.Format("data:image/gif;base64,{0}", base64);
		}
	}
}
