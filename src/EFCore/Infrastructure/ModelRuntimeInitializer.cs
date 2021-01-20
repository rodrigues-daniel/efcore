// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

#nullable enable

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    /// <summary>
    ///     <para>
    ///         Initializes a <see cref="IModel" /> with the runtime dependencies.
    ///     </para>
    ///     <para>
    ///         This type is typically used by database providers (and other extensions). It is generally
    ///         not used in application code.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Singleton" />. This means a single instance
    ///         is used by many <see cref="DbContext" /> instances. The implementation must be thread-safe.
    ///         This service cannot depend on services registered as <see cref="ServiceLifetime.Scoped" />.
    ///     </para>
    /// </summary>
    public class ModelRuntimeInitializer : IModelRuntimeInitializer
    {
        /// <summary>
        ///     Creates a new <see cref="ModelRuntimeInitializer" /> instance.
        /// </summary>
        /// <param name="dependencies"> The dependencies to use. </param>
        public ModelRuntimeInitializer([NotNull] ModelRuntimeInitializerDependencies dependencies)
        {
            Check.NotNull(dependencies, nameof(dependencies));

            Dependencies = dependencies;
        }

        /// <summary>
        ///     The dependencies.
        /// </summary>
        protected virtual ModelRuntimeInitializerDependencies Dependencies { get; }

        /// <summary>
        ///     Validates and initializes the given model with runtime dependencies.
        /// </summary>
        /// <param name="model"> The model to initialize. </param>
        /// <param name="validationLogger"> The validation logger. </param>
        /// <returns> The initialized model. </returns>
        public virtual IModel Initialize(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation>? validationLogger)
        {
            if (model.SetModelDependencies(Dependencies.ModelDependencies))
            {
                InitializeModel(model, preValidation: true);
                if (validationLogger != null
                    && model is IMutableModel)
                {
                    Dependencies.ModelValidator.Validate(model, validationLogger);
                }
                InitializeModel(model, preValidation: false);
            }

            return model;
        }

        /// <summary>
        ///     Initializes the given model with runtime dependencies.
        /// </summary>
        /// <param name="model"> The model to initialize. </param>
        /// <param name="preValidation">
        ///     <see langword="true"/> indicates that only pre-validation initialization should be performed;
        ///     <see langword="false"/> indicates that only post-validation initialization should be performed;
        /// </param>
        protected virtual void InitializeModel([NotNull] IModel model, bool preValidation)
        {
        }
    }
}
