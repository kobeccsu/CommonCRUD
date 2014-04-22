﻿using Microsoft.CSharp;
using MyFirstMVCWebSite.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.Compilation;

namespace MyFirstMVCWebSite.Util
{
    public class DynamicExecuteCode
    {
        public static object ExecuteCode(string code)
        {
            // 1.CSharpCodePrivoder
            //CSharpCodeProvider objCSharpCodePrivoder = 

            // 2.ICodeComplier
            //ICodeCompiler objICodeCompiler = new CSharpCodeProvider();
            CSharpCodeProvider objICodeCompiler = new CSharpCodeProvider();
            

            // 3.CompilerParameters
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;

            // 4.CompilerResults
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, code);

            object objHelloWorld = null;
            if (cr.Errors.HasErrors)
            {
                Console.WriteLine("编译错误：");
                foreach (CompilerError err in cr.Errors)
                {
                    Console.WriteLine(err.ErrorText);
                }
            }
            else
            {
                // 通过反射，调用HelloWorld的实例
                Assembly objAssembly = cr.CompiledAssembly;
                objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
            }
            return objHelloWorld;
        }

        public static object GetMyType(Dictionary<string, string> paramDic)
        {
            string[] namelist = new string[] { "UserName", "UserID" };
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("UserName", "zhoulei");
            //dic.Add("UserID", "1");

            string strDynamicModuleName = "jksdynamic";
            string strDynamicClassName = "<>jksdynamci";
            AppDomain currentDomain = System.AppDomain.CurrentDomain;
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = strDynamicModuleName;

            System.Reflection.Emit.AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(strDynamicModuleName);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(strDynamicClassName, TypeAttributes.Public);

            Type[] methodArgs = { typeof(string) };


            Type[] constructorTypes = new Type[paramDic.Count];
            for (int i = 0; i < paramDic.Count; i++)
            {
                constructorTypes[i] = typeof(string);
            }

            ConstructorBuilder constructorBuiler = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard,
                constructorTypes);

            //            typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard);
            //typeBuilder.d
            //动态创建字段
            // FieldBuilder fb = typeBuilder.DefineField(item, typeof(System.String), FieldAttributes.Private);
            //ILGenerator ilg = constructorBuiler.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
            //ilg.Emit(OpCodes.Ldarg_0);
            //ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            //ilg.Emit(OpCodes.Ldarg_0);
            //ilg.Emit(OpCodes.Ldarg_1);

            ////ilg.Emit(OpCodes.Stfld);
            //ilg.Emit(OpCodes.Ret);

            int index = 0;
            ILGenerator ilg = constructorBuiler.GetILGenerator();
            foreach (string item in paramDic.Keys)
            {

                //typeBuilder.DefineConstructor(MethodAttributes.Assembly, CallingConventions.VarArgs, new Type[] { typeof(string), typeof(int) });

                //动态创建字段
                //FieldBuilder fb = typeBuilder.DefineField("_" + item, dic[item], FieldAttributes.Private); 

                //类型的属性成员由两部分组成，一是私有字段，二是访问私有字段的属性包装器。
                //包装器运行时的本质与 Method 一样，即包含 Set_Method 和 Get_Method 两个方法。
                //动态创建字段
                FieldBuilder fieldBuilder = typeBuilder.DefineField(item, typeof(string), FieldAttributes.Public);

                //FieldBuilder conFieldBuilder = typeBuilder.DefineField(item.ToLower(), dic[item], FieldAttributes.Public);


                index++;
                ilg.Emit(OpCodes.Ldarg_0);//向MSIL流发送属性实例
                ilg.Emit(OpCodes.Ldarg, index);//将指定索引的参数加到堆栈上。
                ilg.Emit(OpCodes.Stfld, fieldBuilder);//装载字段



                //ilg.Emit(OpCodes.Stfld, fieldBuilder);

                //PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(item, PropertyAttributes.None, dic[item], null);
                ////MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
                //MethodBuilder methodBuilder = typeBuilder.DefineMethod("get_" + item, MethodAttributes.Public, dic[item], null);

                //ILGenerator ilGenerator = methodBuilder.GetILGenerator();
                //ilGenerator.Emit(OpCodes.Ldarg_0);
                //ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);//装载属性私有字段
                //ilGenerator.Emit(OpCodes.Ret);
                //propertyBuilder.SetGetMethod(methodBuilder);// 设置获取属性值的方法

                //methodBuilder = typeBuilder.DefineMethod("set_" + item,
                //               MethodAttributes.Public,
                //               typeof(void), new Type[] { dic[item] });

                //ilGenerator = methodBuilder.GetILGenerator();
                //ilGenerator.Emit(OpCodes.Ldarg_0);
                //ilGenerator.Emit(OpCodes.Ldarg_1);
                //ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                //ilGenerator.Emit(OpCodes.Ret);
                //propertyBuilder.SetSetMethod(methodBuilder);// 设置属性值的方法


            }
            ilg.Emit(OpCodes.Ret);
            Type type = typeBuilder.CreateType();

            Type typeDynamic = moduleBuilder.GetType(strDynamicClassName);

            string[] paravalue = new string[paramDic.Count];

            int tempi = 0;
            foreach (var item in paramDic.Values)
            {
                paravalue[tempi++] = item;
            }
            
            object objReturn = Activator.CreateInstance(typeDynamic, paravalue);

            //object objReturn = Activator.CreateInstance(typeDynamic);

            return objReturn;

        }

        public static IQueryable<T> BuildWhere<T>(northwindContext db, NameValueCollection queryString) where T : class
        {
            IQueryable<T> custs = db.Set<T>();
            // 创建一个参数c
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            //c.City=="London"

            Expression pred = Expression.Equal(Expression.Constant(1), Expression.Constant(1)) ;
            foreach (var item in queryString.AllKeys)
            {
                Expression left = Expression.Property(param, typeof(T).GetProperty(item));
                // 类型转换总是不对，垃圾微软
                TypeConverter convert = TypeDescriptor.GetConverter(left.Type);
                
                Expression right = Expression.Constant(convert.ConvertFromString(queryString[item]), left.Type);
                
                Expression filter = Expression.Equal(left, right);
                
                pred = Expression.And(pred, filter);
            }

            //pred = Expression.Lambda(pred, param);

            //Where(c=>c.City=="London")
            //Expression expr = Expression.Call(typeof(Queryable), "Where",
            //    new Type[] { typeof(T) },
            //    Expression.Constant(custs), pred);
            ////生成动态查询
            //IQueryable<T> query = db.Set<T>().AsQueryable()
            //    .Provider.CreateQuery<T>(expr);

            // 表达式，lambda 这两个概念不是太清楚，所以导致这里遇到了阻碍
            var lambda = Expression.Lambda<Func<T, bool>>(pred, param);
            return db.Set<T>().Where<T>(lambda);

            //return query;
        }

        public static object StringToType(string value, Type propertyType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType == null)
                return Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture);
            return String.IsNullOrEmpty(value)
                   ? null
                   : Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
        }
    }


}