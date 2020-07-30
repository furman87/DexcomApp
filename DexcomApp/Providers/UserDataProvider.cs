// <copyright file="UserDataProvider.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DexcomApp.Models;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides basic data functionality for a user.
    /// </summary>
    public class UserDataProvider : IDataProvider<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataProvider"/> class.
        /// </summary>
        /// <param name="config">Configuration object.</param>
        public UserDataProvider(IConfiguration config)
        {
            this.Configuration = config;
        }

        private IConfiguration Configuration { get; }

        /// <inheritdoc/>
        public int Create(User data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public User Read(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Update(User data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
