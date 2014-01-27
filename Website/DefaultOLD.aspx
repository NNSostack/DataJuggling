<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultOLD.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="plunker">
<head runat="server">
    <title></title>
    <script src="http://code.angularjs.org/1.2.5/angular.min.js"></script>
    <script src="App/Controllers/dataJoggeling.js"></script>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.2.0rc1/angular-sanitize.min.js""></script>

    <script src="Scripts/bootstrap.min.js"></script>
    <link rel="stylesheet" type="text/css" href="Content/bootstrap-theme.min.css" />
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />

    <style>
        #main {
            padding: 10px;
            width: 1500px;
        }

        #mainForm {
            width: 500px;
        }

        .form-group {
            padding-bottom: 10px;
        }

        .fade {
            opacity: 0;
            -webkit-transition: opacity 0.25s ease-in;
            -moz-transition: opacity 0.25s ease-in;
            -o-transition: opacity 0.25s ease-in;
            -ms-transition: opacity 0.25s ease-in;
            transition: opacity 0.25s ease-in;
        }

            .fade.in {
                opacity: 1;
            }
    </style>

</head>
<body>
    <div id="main" ng-controller="dataJoggeling">
        <form role="form" id="mainForm" style="float:left;">

            <div class="tabbable">
                <!-- Only required for left/right tabs -->
                <ul class="nav nav-tabs">
                    <li class="active"><a href="#tab1" data-toggle="tab">CSV data</a></li>
                    <li data-ng-show="csvData.CsvDocumentValid"><a href="#tab4" data-toggle="tab">Opsætning</a></li>
                    <li data-ng-show="csvData.CsvDocumentValid"><a href="#tab2" data-toggle="tab">Gruppering</a></li>
                    <li data-ng-show="csvData.CsvDocumentValid"><a href="#tab3" data-toggle="tab">Filtrering</a></li>
                    <li data-ng-show="csvData.DisplayType == 'Timeline'"><a href="#tab5" data-toggle="tab">Tidsline</a></li>
                    <li data-ng-show="csvData.DisplayType == 'List'"><a href="#tab6" data-toggle="tab">Liste</a></li>
                    
                    
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="tab1">
                        <div data-ng-include src="'Views/Analysis.html'"></div>
                    </div>
                    

                    <%--Gruppering--%>
                    <div class="tab-pane" id="tab2">
                        <div data-ng-include src="'Views/Grouping.html'"></div>
                    </div>
                    <%--Filtrering--%>
                    <div class="tab-pane" id="tab3">
                        <div data-ng-include src="'Views/Filtering.html'"></div>
                    </div>
                    <%--Opsætningstype--%>
                    <div class="tab-pane" id="tab4">
                        <div data-ng-include src="'Views/Setup.html'"></div>
                    </div>    
                    <%--Timeline--%>
                    <div class="tab-pane" id="tab5">
                        <div data-ng-include src="'Views/Timeline.html'"></div>
                    </div>
                    <%--List--%>
                    <div class="tab-pane" id="tab6">
                        <div data-ng-include src="'Views/List.html'"></div>
                    </div>

                    <button type="button" class="btn btn-default" data-ng-click="changePreview()">{{ previewText() }}</button>
                </div>
            </div>

        </form>
        <div class="clearfix"></div>
        
        <div ng-show="preview">
            <iframe ng-src="{{iFrameTrustSrc}}" frameborder="0" style="float:left;width:900px;height:500px"></iframe>
            <input type="text" value="{{iFrameTrustSrc}}" style="width:900px;" />
        </div>

    </div>


</body>
</html>
