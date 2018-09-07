// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace IdentityServer.Controllers.Account
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public string TenantName { get; set; }
        public bool EnableLocalLogin { get; set; }
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
    }
    
    public class ExternalProvider
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}
