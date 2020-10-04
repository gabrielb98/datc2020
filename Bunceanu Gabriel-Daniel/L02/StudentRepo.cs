using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPT_Students
{
    public static class StudentRepo
    {
        public static List<Student> students = new List<Student>(){
	    new Student() {StudentID=1, LastName="Bunceanu", FirstName="Gabriel", Faculty="AC", StudyYear = 4},
	    new Student() {StudentID=2, LastName="Toma", FirstName="Cristian", Faculty="AC", StudyYear = 1},
	    new Student() {StudentID=3, LastName="Radu", FirstName="Ana", Faculty="ETC", StudyYear = 2},
	    new Student() {StudentID=4, LastName="Alex", FirstName="Marcel", Faculty="Mecanica", StudyYear = 3}
    };

        
    }
}