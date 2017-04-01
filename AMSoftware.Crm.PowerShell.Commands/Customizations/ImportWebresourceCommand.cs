﻿/*
CRM PowerShell Library
Copyright (C) 2017 Arjan Meskers / AMSoftware

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Management.Automation;
using AMSoftware.Crm.PowerShell.Common.Repositories;
using Microsoft.PowerShell.Commands;
using Microsoft.Xrm.Sdk;

namespace AMSoftware.Crm.PowerShell.Commands.Customizations
{
    [Cmdlet(VerbsData.Import, "Webresource", HelpUri = HelpUrlConstants.ImportWebresourceHelpUrl)]
    public class ImportWebresourceCommand : CrmOrganizationCmdlet, IDynamicParameters
    {
        internal const string ImportWebresourceFromValueParameterSet = "ImportWebresourceFromValue";
        internal const string ImportWebresourceFromPathParameterSet = "ImportWebresourceFromPath";

        private ContentRepository _repository = new ContentRepository();
        private ImportWebresourceDynamicParameters _contentParameters = new ImportWebresourceDynamicParameters();

        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        [ValidateNotNull]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ImportWebresourceFromValueParameterSet)]
        [ValidateNotNull]
        public byte[] Value { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ImportWebresourceFromPathParameterSet)]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        public object GetDynamicParameters()
        {
            return _contentParameters;
        }

        protected override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            Entity webresource = _repository.Get("webresource", Id, new string[] { "content" });

            switch (this.ParameterSetName)
            {
                case ImportWebresourceFromValueParameterSet:
                    string contentAsBase64 = Convert.ToBase64String(Value);
                    webresource.Attributes["content"] = contentAsBase64;
                    break;
                case ImportWebresourceFromPathParameterSet:
                    FileContentReaderWriter fcrw = new FileContentReaderWriter(Path, _contentParameters.EncodingType, _contentParameters.UsingByteEncoding);
                    byte[] fileAsBytes = fcrw.ReadAsBytes();
                    string fileContentAsBase64 = Convert.ToBase64String(fileAsBytes);
                    webresource.Attributes["content"] = fileContentAsBase64;
                    break;
            }

            _repository.Update(webresource);
        }
    }

    public class ImportWebresourceDynamicParameters : FileContentDynamicsParameters
    {

        [Parameter(ParameterSetName = ImportWebresourceCommand.ImportWebresourceFromPathParameterSet)]
        public FileSystemCmdletProviderEncoding Encoding
        {
            get
            {
                return base.streamType;
            }
            set
            {
                base.streamType = value;
            }
        }
    }
}
