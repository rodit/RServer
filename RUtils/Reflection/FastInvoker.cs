using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace RUtils.Reflection
{
    public delegate object FastInvokeHandler(object target, params object[] parameters);
	public delegate object FastConstructorHandler(params object[] parameters);

	public static class MethodInvoker
	{
		public static FastConstructorHandler BindFastConstructor(this ConstructorInfo ctrInfo)
		{
			var dynamicMethod = new DynamicMethod($"FastConstructor_{ctrInfo.Name}", typeof(object), new Type[] { typeof(object[]) });

			var il = dynamicMethod.GetILGenerator();

			il.Emit(OpCodes.Nop);

			var ctrParams = ctrInfo.GetParameters();
			for (int i = 0; i < ctrParams.Length; i++)
			{
				var paramType = ctrParams[i].ParameterType;
				il.Emit(OpCodes.Ldarg_0);
				EmitFastInt(il, i);
				il.Emit(OpCodes.Ldelem_Ref);
				if (paramType.IsValueType)
				{
					il.Emit(OpCodes.Unbox_Any);
				}
				else
				{
					il.Emit(OpCodes.Castclass, paramType);
				}
			}
			il.Emit(OpCodes.Newobj, ctrInfo);
			il.Emit(OpCodes.Ret);

			return (FastConstructorHandler)dynamicMethod.CreateDelegate(typeof(FastConstructorHandler));
		}

		public static FastInvokeHandler BindSetProperty(this Type type, Predicate<PropertyInfo> pred)
		{
			var prop = type.GetProperties().FirstOrDefault(p => pred(p));
			if (prop != null)
			{
				return prop.SetMethod.BindFastInvoker();
			}
			return null;
		}

		public static FastInvokeHandler BindFastInvoker(this MethodInfo methodInfo, bool directBoxValueAccess = false)
		{
			var dynamicMethod = new DynamicMethod($"FastInvoke_{methodInfo.Name}_{(directBoxValueAccess ? "direct" : "indirect")}", typeof(object), new Type[] { typeof(object), typeof(object[]) });
			var il = dynamicMethod.GetILGenerator();

			if (!methodInfo.IsStatic)
			{
				Emit(il, OpCodes.Ldarg_0);
				EmitUnboxIfNeeded(il, methodInfo.DeclaringType);
			}

			var generateLocalBoxObject = true;
			var ps = methodInfo.GetParameters();
			for (var i = 0; i < ps.Length; i++)
			{
				var argType = ps[i].ParameterType;
				var argIsByRef = argType.IsByRef;
				if (argIsByRef)
					argType = argType.GetElementType();
				var argIsValueType = argType.IsValueType;

				if (argIsByRef && argIsValueType && !directBoxValueAccess)
				{
					// used later when storing back the reference to the new box in the array.
					Emit(il, OpCodes.Ldarg_1);
					EmitFastInt(il, i);
				}

				Emit(il, OpCodes.Ldarg_1);
				EmitFastInt(il, i);

				if (argIsByRef && !argIsValueType)
				{
					Emit(il, OpCodes.Ldelema, typeof(object));
				}
				else
				{
					Emit(il, OpCodes.Ldelem_Ref);
					if (argIsValueType)
					{
						if (!argIsByRef || !directBoxValueAccess)
						{
							// if !directBoxValueAccess, create a new box if required
							Emit(il, OpCodes.Unbox_Any, argType);
							if (argIsByRef)
							{
								// the following ensures that any references to the boxed value still retain the same boxed value,
								// and that only the boxed value within the parameters array can be changed
								// this is done by "reboxing" the value and replacing the original boxed value in the parameters array with this reboxed value

								// box back
								Emit(il, OpCodes.Box, argType);

								// for later stelem.ref
								Emit(il, OpCodes.Dup);

								// store the "rebox" in an object local
								if (generateLocalBoxObject)
								{
									generateLocalBoxObject = false;
									_ = il.DeclareLocal(typeof(object), false);
								}
								Emit(il, OpCodes.Stloc_0);

								// arr and index set up already
								Emit(il, OpCodes.Stelem_Ref);

								// load the "rebox" and emit unbox (get unboxed value address)
								Emit(il, OpCodes.Ldloc_0);
								Emit(il, OpCodes.Unbox, argType);
							}
						}
						else
						{
							// if directBoxValueAccess, emit unbox (get value address)
							Emit(il, OpCodes.Unbox, argType);
						}
					}
				}
			}

			if (methodInfo.IsStatic)
				EmitCall(il, OpCodes.Call, methodInfo);
			else
				EmitCall(il, OpCodes.Callvirt, methodInfo);

			if (methodInfo.ReturnType == typeof(void))
				Emit(il, OpCodes.Ldnull);
			else
				EmitBoxIfNeeded(il, methodInfo.ReturnType);

			Emit(il, OpCodes.Ret);

			return (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
		}

		internal static void Emit(ILGenerator il, OpCode opcode)
		{
			il.Emit(opcode);
		}

		internal static void Emit(ILGenerator il, OpCode opcode, Type type)
		{
			il.Emit(opcode, type);
		}

		internal static void EmitCall(ILGenerator il, OpCode opcode, MethodInfo methodInfo)
		{
			il.EmitCall(opcode, methodInfo, null);
		}

		static void EmitUnboxIfNeeded(ILGenerator il, Type type)
		{
			if (type.IsValueType)
				Emit(il, OpCodes.Unbox_Any, type);
		}

		static void EmitBoxIfNeeded(ILGenerator il, Type type)
		{
			if (type.IsValueType)
				Emit(il, OpCodes.Box, type);
		}

		internal static void EmitFastInt(ILGenerator il, int value)
		{
			switch (value)
			{
				case -1:
					il.Emit(OpCodes.Ldc_I4_M1);
					return;
				case 0:
					il.Emit(OpCodes.Ldc_I4_0);
					return;
				case 1:
					il.Emit(OpCodes.Ldc_I4_1);
					return;
				case 2:
					il.Emit(OpCodes.Ldc_I4_2);
					return;
				case 3:
					il.Emit(OpCodes.Ldc_I4_3);
					return;
				case 4:
					il.Emit(OpCodes.Ldc_I4_4);
					return;
				case 5:
					il.Emit(OpCodes.Ldc_I4_5);
					return;
				case 6:
					il.Emit(OpCodes.Ldc_I4_6);
					return;
				case 7:
					il.Emit(OpCodes.Ldc_I4_7);
					return;
				case 8:
					il.Emit(OpCodes.Ldc_I4_8);
					return;
			}

			if (value > -129 && value < 128)
				il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
			else
				il.Emit(OpCodes.Ldc_I4, value);
		}
	}
}
