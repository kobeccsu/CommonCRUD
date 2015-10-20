using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstMVCWebSite.Models;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;
using System.Data.Objects;
using MyFirstMVCWebSite.DAL;
using MyFirstMVCWebSite.Util;
using System.Web.Routing;
using System.ComponentModel;
using PagedList;
using PagedList.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Objects.DataClasses;
//using System.Linq.Dynamic;

namespace MyFirstMVCWebSite.Controllers
{
    public class CommonCRUDController : Controller
    {
        //
        // GET: /CommonCRUD/
        northwindContext db = new northwindContext();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            Test();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            /*"new(Quantity,OrderID, ProductID)"*/
            var query =
                db.Order_Details.Select(c => new { c.ProductID, c.OrderID, c.Quantity }).OrderBy(c=>c.OrderID);

            //var query =
            //    db.Order_Details.Select("new(Quantity,OrderID, ProductID)").OrderBy("OrderID");

                //from a in db.order_details
                //select new { a.product, a.quantity, a.orderid };
            
            ViewBag.Keys = GetKeyNames<Order_Detail>(db);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.Data = query.ToPagedList(pageNumber, pageSize);

            //ViewBag.Data = query.Skip(pageNumber * 10).Take(10);

            //string zhoule = "sdf"; 
            //Expression filter = (n => n * n);
            //var One = filter.Compile();
            //Console.WriteLine("Result: {0},{1}", One(5), One(1));
            //ViewBag.text = zhoule.GetMassWord(2);

            return View(ViewBag.Data);
        }

        //[HttpPost]
        //public ActionResult Index(int? past)
        //{
        //    var query = from a in db.Products
        //                select a;
        //    ViewBag.Keys = GetKeyNames<Product>(db);
        //    ViewBag.Data = query.ToList();
        //    return View();
        //}

        /// <summary>
        /// 找到某个对象的key
        /// </summary>
        /// <typeparam name="T">dbset,如 product </typeparam>
        /// <param name="context">数据上下文</param>
        /// <returns></returns>
        public string[] GetKeyNames<T>(DbContext context) where T : class
        {
            ObjectSet<T> objectSet = ((IObjectContextAdapter)context).ObjectContext.CreateObjectSet<T>();
            string[] keyNames = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToArray();
            return keyNames;
        }

        public object[] GetKeyValues<T>(T entity, DbContext context) where T : class
        {
            var keyNames = GetKeyNames<T>(context);
            Type type = typeof(T);

            object[] keys = new object[keyNames.Length];
            for (int i = 0; i < keyNames.Length; i++)
            {
                keys[i] = type.GetProperty(keyNames[i]).GetValue(entity, null);
            }
            return keys;
        }

        //[HttpPost]
        //[MultiButton(Name="delete")]
        //public ActionResult Delete(object id)
        //{
        //    if (db.Entry(db.Products.Find(id)).State == EntityState.Detached)
        //        db.Products.Attach(db.Products.Find(id));
        //    return View();
        //}

        [HttpPost]
        public ActionResult Add(object obj)
        {
            //Order_Detail = new Order_Detail() { OrderID =  };

            StringBuilder sb = new StringBuilder();
            var properties = typeof(Order_Detail).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
     
            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.GetMethod.IsVirtual)
                {
                    sb.Append(propertyInfo.Name + ",");
                }
            }
            //todo: 这里需要动态的创建对应的类，然后插入

            GenericRepository<Order_Detail> dal = new GenericRepository<Order_Detail>(db);
            dal.Insert(new Order_Detail { OrderID = int.Parse(Request.Form["OrderId"]) });
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(RouteValueDictionary objs)
        {
            //var createObj = DynamicExecuteCode.ExecuteCode("var tempParam = new {title = \"sdgd\", name = \"12324\"}");
            //var createObj = DynamicExecuteCode.GetMyType(obj);
            //object[] parameters = new object[Request.QueryString.Count];
            //for (int i = 0; i < Request.QueryString.Count; i++)
            //{
            //    parameters[i] = Request.QueryString[Request.QueryString.AllKeys[i]];
            //}
            GenericRepository<Order_Detail> dal = new GenericRepository<Order_Detail>(db);

            //TypeDescriptor.GetConverter(typeof(T)).CanConvertFrom(typeof(string));
            Order_Detail findObj = DynamicExecuteCode.BuildWhere<Order_Detail>(db, Request.QueryString).SingleOrDefault();// db.Order_Details.Find(parameters);
            dal.Delete(findObj);
            db.SaveChanges();

            //Product findP = db.Products.Find(id);
            //if (db.Entry(findP).State == EntityState.Detached)
            //    db.Products.Attach(db.Products.Find(id));
            //db.Products.Remove(findP);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void Test()
        {
            // 添加你要测试的内容到 ViewBag 这里会在页面的最下面显示

            //ViewBag.text = sb.ToString();
        }

        delegate int del(int i);

    }
}
