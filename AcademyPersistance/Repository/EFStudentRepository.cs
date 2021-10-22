
using Microsoft.EntityFrameworkCore;
using AcademyModel.Entities;
using AcademyModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademyModel;
using AcademyEFPersistance.EFContext;
using AcademyModel.BuisnessLogic;

namespace AcademyEFPersistance.Repository {
	public class EFStudentRepository : EFCrudRepository<Student, long>, IStudentRepository
	{
		public EFStudentRepository(AcademyContext ctx) : base(ctx)
		{

		}
		public IEnumerable<Student> FindByLastnameLike(string lastnameLike)
		{
			return ctx.Students.Where(a => a.Lastname.Contains(lastnameLike));
		}

		public IEnumerable<Student> FindStudentByCompetence(long idSkill, Level? level)
		{
			IEnumerable<Student> students = new List<Student>();
			if (level == null)
			{
				students = ctx.Students.Where(i => i.Competences.Any(c => c.SkillId == idSkill));
			}
			else
			{
				students = ctx.Students.Where(i => i.Competences.Any(c => c.Level >= level && c.SkillId == idSkill));
			}
			return students;
		}

		public IEnumerable<Student> SearchDetailed(StudentSearchInfo info)
        {
			List<string> splitted = new List<string>();
			IQueryable<Student> students = ctx.Students;
			if (info.Fullname != null) splitted = info.Fullname.Split(' ').ToList();
			if (splitted.Count == 2)
			{
				if (splitted[0] != null || splitted[1] != null)
				{
					if (splitted[0] != null)
					{
						students = students.Where(s => s.Firstname == splitted[0] || s.Firstname.Contains(splitted[0]));
					}
					else if (splitted[1] != null)
					{
						students = students.Where(s => s.Lastname == splitted[1] || s.Lastname.Contains(splitted[1]));
					}
				}
			}
			else if (splitted.Count == 1)
			{
				students = students.Where(s => s.Firstname == splitted[0] || s.Firstname.Contains(splitted[0])
												|| s.Lastname == splitted[0] || s.Lastname.Contains(splitted[0]));
			}
			return students;
        } 
	}
}
