using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using WorkoutPlanObjects;
using System.Web;



namespace WorkoutPlanAPI
{
    public class PrintToPDF
    {
        BaseFont normalFont = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        Font boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
        int pageNumber = 1;

        public PrintToPDF()
        {

        }

        public void PrintWorkoutPlan(WorkoutPlan workoutPlan)
        {
            Document document = new Document(PageSize.A4);
            System.IO.MemoryStream msReport = new System.IO.MemoryStream();  // for asp.net

            try
            {

                PdfWriter writer = PdfWriter.GetInstance(document, msReport);
                document.Open();

                PdfContentByte cb = writer.DirectContent;
                PrintFrame(document, cb, workoutPlan);
                PrintContent(document,cb, workoutPlan);

            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }
        
            document.Close();
            
            HttpContext.Current.Response.Clear(); 
            HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=Export.pdf"); //use inline if want to open in browser itself, else attachment to save it.
            HttpContext.Current.Response.ContentType = "application/pdf"; 
            HttpContext.Current.Response.BinaryWrite(msReport.ToArray()); 
            HttpContext.Current.Response.End();
             
        }


        void PrintFrame(Document doc, PdfContentByte cb, WorkoutPlan wp)
        {

            cb.SetLineWidth(1.0f);   // Make a bit thicker than 1.0 default
            cb.SetColorStroke(BaseColor.BLACK);
            cb.MoveTo(40, 40);    // move to start point
            cb.LineTo(560, 40);   // draw to end point, or continue to draw to second end point
            cb.LineTo(560, 700 );
            cb.LineTo(40 ,700);
            cb.LineTo(40, 40);   // or cb.ClosePath();
            
            cb.Stroke();    // finish line


            cb.BeginText();
            cb.SetFontAndSize(boldFont.BaseFont, 20);
            cb.SetTextMatrix(200, 780);
            cb.ShowText(string.Format("Swimming Workout Plan"));


            cb.SetFontAndSize(boldFont.BaseFont, 15);
            cb.SetTextMatrix(40, 720);
            cb.ShowText(string.Format("Date: " +wp.Date).Substring(0,16));
            

            // we draw some text on a certain position
            //cb.SetTextMatrix(50,780);
            //cb.ShowText(string.Format("Team-{0}    Week-{1}    Day-{2}",wp.TeamName,wp.ScheduleWeek,wp.ScheduleWeekDay));

            //cb.SetTextMatrix(240, 720);
            //cb.ShowText(string.Format("Workout Plan", wp.ScheduleWeek, wp.ScheduleWeekDay));

            cb.SetTextMatrix(540,25);
            cb.SetFontAndSize(normalFont, 8);
            cb.ShowText("page " + pageNumber);
            pageNumber++;

            // we tell the contentByte, we've finished drawing text
            cb.EndText();
        }

        void PrintContent(Document doc, PdfContentByte cb, WorkoutPlan wp)
        {
            int current_y = 660;    //start from top to bottom
            int row_gap = 20;
            int indent_80 = 80;
            int indent_100 = 100;
            cb.BeginText();
            cb.SetFontAndSize(boldFont.BaseFont, 15);
            cb.SetTextMatrix(indent_80, current_y + 20);
            cb.ShowText("TYMS             Week             Day");
                foreach (var item in wp.WorkoutSet)
                {
                   // print item
                        cb.SetTextMatrix(indent_100, current_y); current_y = current_y - row_gap;

                    //logic whether to display the pace time or rest time or none at all
                        if (item.PaceTime == 0 ) //pace time is checked first because it is most likely to be used the most
                        {
                            if (item.RestPeriod ==0 )
	                            {
                                    cb.SetFontAndSize(normalFont, 10);
                                    //cb.ShowText(string.Format("{0,-10} X {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10} {10,10} {11,10} ",
                                      //  item.Repeats, item.WorkoutSetDistance, item.SingleDuration, item.Stroke.Name, item.Description, item.E1 , item.E2 , item.E3, item.S1,item.S2,item.S3,item.REC)); 
                                    cb.ShowText(string.Format("{0,-4} X {1,4} m    {2,4} {3,15}  {4,20} min  ",
                                        item.Repeats, item.WorkoutSetDistance, item.Stroke.Name, item.Description, item.SingleDuration)); 
	                            }
                            else
	                            {
                                    cb.SetFontAndSize(normalFont, 10);
                                    //cb.ShowText(string.Format("{0,-10} X {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10} {10,10} {11,10} {12,10} min ",
                                     //   item.Repeats, item.WorkoutSetDistance, item.SingleDuration, item.Stroke.Name, item.Description,item.RestPeriod, item.E1, item.E2, item.E3, item.S1, item.S2, item.S3, item.REC)); 
                                    cb.ShowText(string.Format("{0,-4} X {1,4} m    {2,4} {3,15}  on {4,4} {5,20} min  ",
                                        item.Repeats, item.WorkoutSetDistance, item.Stroke.Name, item.Description, item.RestPeriod, item.SingleDuration)); 
	                            }
                          
                        }
                        else
                        {
                            cb.SetFontAndSize(normalFont, 10);
                           // cb.ShowText(string.Format("{0,4} X {1,4} {2,4} {3,4} {4,10} {5,4} {6,4} {7,4} {8,4} {9,4} {10,4} {11,4} {12,4} ",
                            //   item.Repeats, item.WorkoutSetDistance, item.SingleDuration, item.Stroke.Name, item.Description, item.PaceTime, item.E1, item.E2, item.E3, item.S1, item.S2, item.S3, item.REC));

                            cb.ShowText(string.Format("{0,-4} X {1,4} m    {2,4} {3,15}  on {4,4} {5,20} min  ",
                                 item.Repeats, item.WorkoutSetDistance, item.Stroke.Name, item.Description, item.PaceTime,item.SingleDuration)); 
                        }

                    
                    if (current_y < 80)
                    {   // start a new page;
                        cb.EndText();
                        doc.NewPage();
                        PrintFrame(doc, cb, wp);
                        cb.BeginText();
                        cb.SetFontAndSize(normalFont, 10);
                        current_y = 680;
                    }
                }
            
            cb.SetTextMatrix(indent_80, current_y - 20); current_y = current_y - row_gap;
            cb.SetFontAndSize(boldFont.BaseFont, 12);
            cb.ShowText(string.Format("Total Duration: {0,10} minutes", wp.WorkoutSet.Sum(a => a.TotalDuration)));
            cb.EndText();
        }
        
    }
}
