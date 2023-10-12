using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Infrastructure.Services
{
    public class StampRequestForm
    {
        public string[] Texts { get; set; }
        public FileStream FileStream { get; set; }

    }
    public class PdfService
    {
        private const int FontSize = 12;
        private readonly BaseFont Font = BaseFont.CreateFont();
        private readonly BaseColor Color = new BaseColor(255, 0, 0);
        private const int VerticalSpaceBetweenLines = 10;
        public Stream ApplyStamp(StampRequestForm stampRequest)
        {
            Stream pdfOutStream = null;
            PdfReader reader = null;
            PdfStamper stamper = null;
            try
            {
                pdfOutStream = new MemoryStream();
                Stream pdfInstream = stampRequest.FileStream;
                reader = new PdfReader(pdfInstream);
                stamper = new PdfStamper(reader, pdfOutStream);
                int pages = reader.NumberOfPages;
                for (int i = 1; i <= pages; i++)
                {
                    var dc = stamper.GetOverContent(i);
                    var realPageSize = reader.GetPageSizeWithRotation(i);

                    AddWaterMark(dc, reader, stampRequest, realPageSize);
                }

            }
            finally
            {
                // pdfInstream?.Close();
                reader?.Close();
                stamper?.Close();

            }
            pdfOutStream.Position = 0;

            return pdfOutStream;
        }

        private void AddWaterMark(PdfContentByte dc, PdfReader reader, StampRequestForm stampRequest,
            iTextSharp.text.Rectangle realPageSize)
        {
            var gstate = new PdfGState
            {
                FillOpacity = 0.3f,
                // StrokeOpacity = 1f
            };
            dc.SaveState();
            dc.SetGState(gstate);
            dc.SetColorFill(Color);
            dc.BeginText();

            if (stampRequest.Texts.Any())
            {
                for (int y = 0; y < 50; y++)
                {
                    var isEven = y % 2 == 0;

                    for (int x = 0; x < 20; x++)
                    {

                        float leftMargin = isEven ? 30 : 90;
                        float xxNew = (realPageSize.Left + leftMargin) + 120 * x;
                        float yyNew = (realPageSize.Top - 20) - 70 * y;

                        var index = 0;
                        foreach (var text in stampRequest.Texts)
                        {
                            if (index != 0)
                            {
                                dc.SetFontAndSize(Font, 10);

                            }
                            else
                            {
                                dc.SetFontAndSize(Font, FontSize);

                            }
                            dc.ShowTextAligned(Element.ALIGN_CENTER,
                                text, xxNew, yyNew, 30);
                            xxNew += 5;
                            yyNew -= 10;
                            index++;
                        }

                    }
                }
            }

            dc.EndText();
            dc.RestoreState();
        }


    }

}


