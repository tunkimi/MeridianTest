using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var teachers = new List<Teacher>(); // Заполнить из файла Учетиля.txt
        var students = new List<Student>(); // Заполнить из файла Ученики.txt
        var exams = new List<Exams>();

        //1. Найти учителя у которого в классе меньше всего учеников 
        //2. Найти средний бал экзамена по Физики за 2023 год.		
        //3. Получить количество учиников которые по экзамену Математики получили больше 90 баллов, где учитель Alex 
        
        using (var sr = new StreamReader(@"D:\Projects\github\MeridianTest\Учетиля.txt"))
        {
            try
            {
                sr.ReadLine();
                var i = 1;
                while (!sr.EndOfStream)
                {
                    var data = sr.ReadLine().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    teachers.Add(new Teacher { ID = i++, Name = data[0], LastName = data[1], Age = int.Parse(data[2]), 
                        Lesson = (data[3].Contains("Математика"))?LessonType.Mathematics:LessonType.Physics});
                }

            }
            catch
            {
                Console.WriteLine("Некорректные данные");
            }

        }
        using (var sr = new StreamReader(@"D:\Projects\github\MeridianTest\Ученики.txt"))
        {
            try
            {
                sr.ReadLine();
                var i = 1;
                while (!sr.EndOfStream)
                {
                    var data = sr.ReadLine().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    students.Add(new Student
                    {
                        ID = i++,
                        Name = data[0],
                        LastName = data[1],
                        Age = int.Parse(data[2]),
                    });
                }

            }
            catch
            {
                Console.WriteLine("Некорректные данные");
            }

        }
        exams.AddRange(new Exams[]
        {
            new Exams
            {
                StudentId = students[0].ID,
                Student = students[0],
                TeacherId = teachers[0].ID,
                Teacher = teachers[0],
                ExamDate = DateTime.Parse("07.01.2023"),
                Lesson = teachers[0].Lesson,
                Score = 87
            },
            new Exams
            {
                StudentId = students[1].ID,
                Student = students[1],
                TeacherId = teachers[2].ID,
                Teacher = teachers[2],
                ExamDate = DateTime.Parse("07.01.2023"),
                Lesson = teachers[2].Lesson,
                Score = 80
            },
            new Exams
            {
                StudentId = students[2].ID,
                Student = students[2],
                TeacherId = teachers[2].ID,
                Teacher = teachers[2],
                ExamDate = DateTime.Parse("07.01.2023"),
                Lesson = teachers[2].Lesson,
                Score = 91
            },
            new Exams
            {
                StudentId = students[0].ID,
                Student = students[0],
                TeacherId = teachers[1].ID,
                Teacher = teachers[1],
                ExamDate = DateTime.Parse("14.01.2023"),
                Lesson = teachers[1].Lesson,
                Score = 91
            },
            new Exams
            {
                StudentId = students[1].ID,
                Student = students[1],
                TeacherId = teachers[1].ID,
                Teacher = teachers[1],
                ExamDate = DateTime.Parse("14.01.2023"),
                Lesson = teachers[1].Lesson,
                Score = 71
            },
            new Exams
            {
                StudentId = students[2].ID,
                Student = students[2],
                TeacherId = teachers[1].ID,
                Teacher = teachers[1],
                ExamDate = DateTime.Parse("14.01.2023"),
                Lesson = teachers[1].Lesson,
                Score = 99
            }
        });



        #region ex1

        var thatTeacher = exams
            .Select(exams => 
                    new { StudentId = exams.StudentId, TeacherId = exams.TeacherId, Teacher = exams.Teacher})
            .Distinct().GroupBy(x => x.TeacherId).MinBy(gr => gr.Count()).Select(gr=>gr.Teacher).First();

        #endregion

        #region ex2

        var avarageScore = exams.Where(e => e.Lesson == LessonType.Physics 
                                         && e.ExamDate < DateTime.Parse("01.01.2024")
                                         && e.ExamDate >= DateTime.Parse("01.01.2023"))
                                .Average(e => e.Score);

        #endregion

        #region ex3

        var thatCount = exams.Where(e => e.Score > 90 
                                      && e.Teacher.Name == "Alex"
                                      && e.Lesson == LessonType.Mathematics).Count();

        #endregion
    }

    public class Person
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class Teacher : Person
    {
        public LessonType Lesson { get; set; }
    }

    public class Student : Person
    {

    }

    public class Exams
    {
        public LessonType Lesson { get; set; }

        public long StudentId { get; set; }
        public long TeacherId { get; set; }

        public decimal Score { get; set; }
        public DateTime ExamDate { get; set; }

        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
    }

    public enum LessonType
    {
        Mathematics = 1,
        Physics = 2
    }
}