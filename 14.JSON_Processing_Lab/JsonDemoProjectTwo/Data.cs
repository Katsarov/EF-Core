using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDemoProjectTwo
{

    public class Rootobject
    {
        public WebApp webapp { get; set; }
    }

    public class WebApp
    {
        public Servlet[] servlet { get; set; }
        public ServletMapping servletmapping { get; set; }
        public Taglib taglib { get; set; }
    }

    public class ServletMapping
    {
        public string cofaxCDS { get; set; }
        public string cofaxEmail { get; set; }
        public string cofaxAdmin { get; set; }
        public string fileServlet { get; set; }
        public string cofaxTools { get; set; }
    }

    public class Taglib
    {
        public string tagliburi { get; set; }
        public string tagliblocation { get; set; }
    }

    public class Servlet
    {
        public string servletname { get; set; }
        public string servletclass { get; set; }
        public InitParam initparam { get; set; }
    }

    public class InitParam
    {
        public string configGlossaryinstallationAt { get; set; }
        public string configGlossaryadminEmail { get; set; }
        public string configGlossarypoweredBy { get; set; }
        public string configGlossarypoweredByIcon { get; set; }
        public string configGlossarystaticPath { get; set; }
        public string templateProcessorClass { get; set; }
        public string templateLoaderClass { get; set; }
        public string templatePath { get; set; }
        public string templateOverridePath { get; set; }
        public string defaultListTemplate { get; set; }
        public string defaultFileTemplate { get; set; }
        public bool useJSP { get; set; }
        public string jspListTemplate { get; set; }
        public string jspFileTemplate { get; set; }
        public int cachePackageTagsTrack { get; set; }
        public int cachePackageTagsStore { get; set; }
        public int cachePackageTagsRefresh { get; set; }
        public int cacheTemplatesTrack { get; set; }
        public int cacheTemplatesStore { get; set; }
        public int cacheTemplatesRefresh { get; set; }
        public int cachePagesTrack { get; set; }
        public int cachePagesStore { get; set; }
        public int cachePagesRefresh { get; set; }
        public int cachePagesDirtyRead { get; set; }
        public string searchEngineListTemplate { get; set; }
        public string searchEngineFileTemplate { get; set; }
        public string searchEngineRobotsDb { get; set; }
        public bool useDataStore { get; set; }
        public string dataStoreClass { get; set; }
        public string redirectionClass { get; set; }
        public string dataStoreName { get; set; }
        public string dataStoreDriver { get; set; }
        public string dataStoreUrl { get; set; }
        public string dataStoreUser { get; set; }
        public string dataStorePassword { get; set; }
        public string dataStoreTestQuery { get; set; }
        public string dataStoreLogFile { get; set; }
        public int dataStoreInitConns { get; set; }
        public int dataStoreMaxConns { get; set; }
        public int dataStoreConnUsageLimit { get; set; }
        public string dataStoreLogLevel { get; set; }
        public int maxUrlLength { get; set; }
        public string mailHost { get; set; }
        public string mailHostOverride { get; set; }
        public int log { get; set; }
        public string logLocation { get; set; }
        public string logMaxSize { get; set; }
        public int dataLog { get; set; }
        public string dataLogLocation { get; set; }
        public string dataLogMaxSize { get; set; }
        public string removePageCache { get; set; }
        public string removeTemplateCache { get; set; }
        public string fileTransferFolder { get; set; }
        public int lookInContext { get; set; }
        public int adminGroupID { get; set; }
        public bool betaServer { get; set; }
    }

}
