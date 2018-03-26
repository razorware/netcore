using System;
using System.Collections;
using System.Collections.Generic;

using RazorWare.Reflection;

namespace RazorWare.Dynamics {
   public interface IModelCache {
      IReadOnlyList<string> PropertyNames { get; }
      IReadOnlyList<TypeData> PropertyTypes { get; }
   }
}
