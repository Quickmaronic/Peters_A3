using Peters_A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peters_A3.Controllers
{
    public class StatesController : Controller
    {
        // GET: States
        /// <summary>
        /// Creates the States Table, with sorting capabilities
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult States(int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<State> allStates = context.States.Where(s => s.IsDeleted == false).ToList();

            switch (sortBy)
            {
                case 1:
                    {
                        allStates = allStates.OrderBy(s => s.StateName).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allStates = allStates.OrderBy(s => s.StateCode).ToList();
                        break;
                    }
            }

            return View(allStates);
        }
        /// <summary>
        /// Creates the State being edited, or sends null if it's a new State
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(string id)
        {
            BooksEntities context = new BooksEntities();
            State state = context.States.Where(s => s.StateCode == id).FirstOrDefault();
            return View(state);
        }

        /// <summary>
        /// Using the created State/null State, it either edits an existing State with the edits, or creates a new State, then saves changes
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(State state)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.States.Where(s => s.StateCode == state.StateCode).Count() > 0)
                {
                    State stateSave = context.States.Where(s => s.StateCode == state.StateCode).FirstOrDefault();

                    stateSave.StateCode = state.StateCode;
                    stateSave.StateName = state.StateName;
                }
                else
                {
                    context.States.Add(state);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("States");
        }

        /// <summary>
        /// Creates the State being deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            State state = context.States.Where(s => s.StateCode == id).FirstOrDefault();
            return View(state);
        }

        /// <summary>
        /// Using the created State, it searches for the matching State and changes the IsDeleted property to true
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(State state)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.States.Where(s => s.StateCode == state.StateCode).Count() > 0)
                {
                    State stateDelete = context.States.Where(s => s.StateCode == state.StateCode).FirstOrDefault();
                    stateDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("States");
        }
    }
}