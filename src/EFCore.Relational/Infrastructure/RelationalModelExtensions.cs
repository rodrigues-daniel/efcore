// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable enable

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    /// <summary>
    ///     Relational-specific extension methods for <see cref="IModel" /> and extension methods for <see cref="IRelationalModel" />.
    /// </summary>
    public static class RelationalModelExtensions
    {
        /// <summary>
        ///     Returns the relational service dependencies.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="methodName"> The name of the calling method. </param>
        /// <returns> The relational service dependencies. </returns>
        public static RelationalModelDependencies GetRelationalDependencies(
            [NotNull] this IModel model, [CallerMemberName][CanBeNull] string methodName = "")
            => (RelationalModelDependencies?)model
                .FindRuntimeAnnotation(RelationalAnnotationNames.ModelDependencies)?.Value
                ?? throw new InvalidOperationException(CoreStrings.ModelNotFinalized(methodName));
    }
}
