using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections;
using System.Drawing;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UsesWhereUsed
{
    public class TreeNode
    {
        Connection _con = null;
        File _file = null;
        WebServiceManager _svc { get { return _con.WebServiceManager; } }
        public string Name { get { return _file.Name; } }
        
        public List<TreeNode> Children { 
            get {
                List<TreeNode> children = new List<TreeNode>();
                FileAssocArray[] fileAssociations = _svc.DocumentService.GetLatestFileAssociationsByMasterIds(new long[] { _file.MasterId }, FileAssociationTypeEnum.None, false, FileAssociationTypeEnum.Dependency, false, false, false,false);
                if (fileAssociations.First().FileAssocs != null)
                    foreach (var fileAssociation in fileAssociations.First().FileAssocs)
                        children.Add(new TreeNode(fileAssociation.CldFile, _con));
                return children;
            }
        }
        
        public List<TreeNode> Parents
        {
            get{
                List<TreeNode> parents = new List<TreeNode>();
                FileAssocArray[] fileAssociations = _svc.DocumentService.GetLatestFileAssociationsByMasterIds(new long[] { _file.MasterId }, FileAssociationTypeEnum.Dependency, false, FileAssociationTypeEnum.None, false, false, false,false);
                if (fileAssociations.First().FileAssocs != null)
                    foreach (var fileAssociation in fileAssociations.First().FileAssocs)
                        parents.Add(new TreeNode(fileAssociation.ParFile, _con));
                return parents;
            }
        }

        public BitmapImage Icon
        {
            get {
                var props = _con.PropertyManager.GetPropertyDefinitions("FILE", null, PropertyDefinitionFilter.IncludeAll);
                var def = props["EntityIcon"];
                var fileIter = new FileIteration(_con,_file);
                ImageInfo prop = _con.PropertyManager.GetPropertyValue(fileIter, def, null) as ImageInfo;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                prop.GetImage().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                prop.Dispose();
                System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
                bImg.BeginInit();
                bImg.StreamSource = ms;// new System.IO.MemoryStream(ms.ToArray());
                bImg.EndInit();
                
                return bImg;
            }
        }

        public TreeNode(File file, Connection con)
        {
            _file = file;
            _con = con;
        }
    }

    //public class ChildTreeNode : TreeNode
    //{
    //    public ChildTreeNode(File file, Connection con)
    //        : base(file, con)
    //    {
    //    }
    //}

    //public class ParentTreeNode : TreeNode
    //{
    //    public ParentTreeNode(File file, Connection con)
    //        : base(file, con)
    //    {
    //    }

    //}
}
