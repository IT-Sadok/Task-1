using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextContentManagement // namespace = a folder analogy for contents within (under) in {...} brackets. 
                                // To invoke/refer:
                                // a) full name use e.g. TextContentManagement.TextContent Title = new TextContentManagement.TextContennt() ; 
                                //               OR
                                // b) using TextContentManagement;
                                //    TextContent Title = new TextContent();
{
    public class TextContent //changed from "internal" to "public" for accessibility.
    {
        // Properties of TextContent
        public string Title { get; set; } // get = read, set = access the property
        public string Author { get; set; }
        public string SerialNumber { get; set; } // string variables because of reference to names.
        public int Year { get; set; } // int for year.
        public bool Availability { get; set; } // bool for "yes" or "no".


            // The TRADITIONAL Constructor, or definition(s) of what's needed to create a TextContent object (title, autor, etc. are paramenets) to prevent errors caused by missing/invalid Properties set by default or by user. In other words - Initializes it "validly".
            // ALSO READ .TXT NOTE ON PRIMARY CONSTRUCTOR!!!

            public TextContent(string title, string author, string serialnumber, int year) 
            {  

            if (string.IsNullOrEmpty(title))
                    throw new ArgumentException("please enter Title.", nameof(title));

            if (string.IsNullOrEmpty(author))
                throw new ArgumentException("please enter Author", nameof(author));

            if (string.IsNullOrEmpty(serialnumber))
                throw new ArgumentException("please enter Serial Number", nameof(serialnumber));
            
           
                    Title = title; // Primary Constructor would look as public string Title { get; set; } = title; giving an option to make it Immutable (not changable) without set;
                    //
                    Author = author; // Capital & lower case letters for organization/clarity, where author is a Parameter given to a Property of the Object that is public TextContent.
                    //
                    SerialNumber = serialnumber; 
                    Year = year; 
                    Availability = true; // default value because TextContent is just created.
            }
    }
}