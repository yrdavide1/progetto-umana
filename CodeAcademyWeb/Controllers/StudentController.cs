﻿using AcademyModel.BuisnessLogic;
using AcademyModel.Entities;
using AcademyModel.Exceptions;
using AcademyModel.Services;
using AutoMapper;
using CodeAcademyWeb.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace CodeAcademyWeb.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : Controller
	{
		private IPeopleService service;
		private IMapper mapper;
		public StudentController(IPeopleService service, IMapper mapper)
		{
			this.service = service;
			this.mapper = mapper;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			//var students = service.GetAllStudents();
			//var studentDTOS = students.Select(s => new StudentDTO(s));
			//return studentDTOS;

			var students = service.GetAllStudents();
			var studentDTOs = mapper.Map<IEnumerable<StudentDTO>>(students);
			return Ok(studentDTOs);
		}

		[HttpGet]
		[Route("name")]
		public IActionResult GetStudentsByFullname([FromQuery] string? fullname)
        {
			StudentSearchInfo searchInfo = new StudentSearchInfo
			{
				Fullname = fullname
			};
			IEnumerable<Student> students = service.FindStudentsDetailed(searchInfo);
			IEnumerable<StudentDTO> studentDTOs = mapper.Map<IEnumerable<StudentDTO>>(students);
			return Ok(studentDTOs);
        }

		[HttpGet]
		[Route("{id}")]
		public IActionResult GetById(long id)
        {
			var singleStudentsDetails = service.GetStudentById(id);
			var studentDTO = mapper.Map<StudentDTO>(singleStudentsDetails);
			return Ok(studentDTO);
        }
		[HttpPost]
		public IActionResult Create(StudentDTO s)
		{
			var student = mapper.Map<Student>(s);
			service.CreateStudent(student);
			var studentDTO = mapper.Map<StudentDTO>(student);
			return Created($"/api/student/{studentDTO.Id}", studentDTO);
		}
		[HttpPost]
		[Route("{idStudent}/enrollments")]
		public IActionResult EnrollStudent(EnrollDataDTO dataDTO, long idStudent)
		{
			if( idStudent != dataDTO.IdStudent )
			{
				return BadRequest(new ErrorObject(StatusCodes.Status400BadRequest, "L'id studente nell'URL e nel body non coincidono."));
			}
			var data = mapper.Map<EnrollData>(dataDTO);
			var enr = service.EnrollSudentToEdition(data);
			var enrDTO = mapper.Map<EnrollmentDTO>(enr);
			return Created($"/api/student/{data.IdStudent}/enrollments/{enr.Id}", enrDTO);
		}
		[HttpPut]
		[Route("{id}")]
		public IActionResult UpdateStudent(StudentDTO s)
        {
			var student = mapper.Map<Student>(s);
			student = service.UpdateStudent(student);
			var resDTO = mapper.Map<StudentDTO>(student);
			return Created($"/api/student/{resDTO.Id}", resDTO);
        }
		[HttpDelete]
		[Route("{id}")]
		public IActionResult DeleteStudent(long id)
        {
			var student = service.GetStudentById(id);
			service.DeleteStudent(student);
			var resDTO = mapper.Map<StudentDTO>(student);
			return Ok(resDTO);
        }
	}
}
