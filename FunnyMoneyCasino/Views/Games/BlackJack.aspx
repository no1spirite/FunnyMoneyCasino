<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="blackJackHead" ContentPlaceHolderID="head" runat="server">
    <title>BlackJack</title>
     <script type="text/javascript" src="/FunnyMoneyCasino/Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
              appSource = sender.getHost().Source;
            }
            
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
              return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {           
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
        function onError(error) 
        {
            alert("Error: " + error.message);
        }
    </script>
</asp:Content>

<asp:Content ID="blackJackContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="b">
<div class="right">
    <h2>Test Silverlight black jack game using MVVM and Ninject</h2>
    <p>Its still a bit buggy so if you find anything please email me at <a href="mailto:ryanxrobinson@yahoo.co.uk">ryanxrobinson@yahoo.co.uk</a>. No Insurance Option yet!! </p>

    <form id="form1" runat="server" style="height:100%;"> 

    <div id="silverlightControlHost" style="height:550px;width:1150px;margin-left:-15px;">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%" HorizontalAlign="Right">
		  <param name="source" value="/ClientBin/BlackJackSL.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="3.0.40624.0" />
		  <param name="autoUpgrade" value="true" />
		  <param name="allowscrolling" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    </form>
            </div>
</div>
</asp:Content>
