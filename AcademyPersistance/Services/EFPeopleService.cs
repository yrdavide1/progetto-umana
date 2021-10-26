﻿using AcademyEFPersistance.EFContext;
using AcademyModel.BuisnessLogic;
using AcademyModel.Entities;
using AcademyModel.Exceptions;
using AcademyModel.Repositories;
using AcademyModel.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEFPersistance.Services
{
	
	public class EFPeopleService : IPeopleService
	{
		private IInstructorRepository instructorRepo;
		private IEditionRepository editionRepo;
		private IStudentRepository studentRepo;
		private AcademyContext ctx;

		public EFPeopleService(IStudentRepository studentRepo, IInstructorRepository instructorRepo, IEditionRepository editionRepo, AcademyContext ctx)
		{
			this.studentRepo = studentRepo;
			this.instructorRepo = instructorRepo;
			this.editionRepo = editionRepo;
			this.ctx = ctx;
		}

		public Student CreateStudent(Student s)
		{
			studentRepo.Create(s);
			ctx.SaveChanges();  //Salviamo qui invece che nella repository
			return s;
		}
		public IEnumerable<Student> GetAllStudents()
		{
			return studentRepo.GetAll().ToList();
		}
		public IEnumerable<Student> GetStudentsByLastnameLike(string lastnameLike)
		{
			return studentRepo.FindByLastnameLike(lastnameLike).ToList(); //Non più una query, ma una lista vera e propria grazie a .ToList
		}
		public Student GetStudentById(long id)
		{
			return studentRepo.FindById(id);
		}
		public Student UpdateStudent(Student s)
		{
			studentRepo.Update(s);
			ctx.SaveChanges();
			return s;
		}
		public void DeleteStudent(Student s)
		{
			studentRepo.Delete(s);
			ctx.SaveChanges();
		}
		public IEnumerable<Student> FindStudentsDetailed(StudentSearchInfo info)
        {
			return studentRepo.SearchDetailed(info).ToList();
        }
		public Enrollment EnrollSudentToEdition(EnrollData data)
		{
			var student = studentRepo.FindById(data.IdStudent);
			if (student == null)
			{
				throw new EntityNotFoundException($"Lo studente con id {data.IdStudent} non esiste.", nameof(Student));
			}
			var edition = editionRepo.FindById(data.IdEdition);
			if (edition == null)
			{
				throw new EntityNotFoundException($"L'edizione con id {data.IdEdition} non esiste.", nameof(CourseEdition));
			}
			var enr = new Enrollment()
			{
				CourseEditionId = edition.Id,
				StudentId = student.Id
			};
			ctx.Enrollments.Add(enr);
			ctx.SaveChanges();
			//ctx.Entry(enr).Reference(e => e.Student).Load();
			//ctx.Entry(enr).Reference(e => e.CourseEdition).Load();
			//ctx.Entry(enr.CourseEdition).Reference(e => e.Course).Load();
			return enr;
		}


		public Instructor GetInstructorById(long id)
		{
			return instructorRepo.FindById(id);
		}

		public IEnumerable<Instructor> GetInstructors()
		{
			return instructorRepo.GetAll();
		}
	}
}
