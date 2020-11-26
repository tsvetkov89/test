using System.Collections.Generic;
using Test.Data;
using Test.Interfaces;
using UnityEngine;

namespace Test.Tools
{
    public class ConfigurationTool : IConfigurationTool
    {
        #region Private Data

        private IObjectLoader _objectLoader;
        private Transform _folder;

        #endregion

        public ConfigurationTool(IObjectLoader objectLoader, Transform folder)
        {
            _objectLoader = objectLoader;
            _folder = folder;
        }

        #region Public Methods

        public GameObject ConfigurateBaseObject(GameObject clone)
        {
            clone.transform.parent = _folder;

            return clone;
        }

        #endregion
    }
}
