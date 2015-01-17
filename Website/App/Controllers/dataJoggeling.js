var app = angular.module('plunker', ['ngSanitize']);


app.controller('dataJoggeling', function($http, $scope, $sce)
{
    $scope._step = 1;

    $scope.columnTypes = [
        { Name: "String" },
        { Name: "DateTime" },
        { Name: "Boolean" },
        { Name: "Url" }
    ];

    $scope.step = function () {
        return $scope._step;
    };

    $scope.incrementStep = function (inc) {
        $scope._step += inc;
        if ($scope._step == 3)
            $scope.changePreview();
    };

    $scope.analyzeCsvData = function () {
        $scope.csvData.isError = false;
        $http.post("/api/Csv/AnalyzeCsvData", $scope.csvData)
            .success(function (data, status, headers, config) {
                $('.has-spinner').toggleClass('active');
                $scope.csvData = data;
                
                if (!data.isError) {
                    $scope.incrementStep(1);
                }
                return data;

            }).error(function (data, status, headers, config) {
                $('.has-spinner').toggleClass('active');
                $scope.csvData = data;
                if (!data.isError) {
                    $scope.incrementStep(1);
                }
                return data;
        });
    };

    $scope.hasError = function () { 
        return $scope.csvData.isError;
    };

    $scope.ErrorMessage = function () {
        return $scope.csvData.ErrorMessage;
    };

    $scope.IsSupported = function (displayType) {
        if ($scope.csvData.NotSupportedDisplayTypes == undefined)
            return false;
        for (var i = 0; i < $scope.csvData.NotSupportedDisplayTypes.length; i++) {
            if ($scope.csvData.NotSupportedDisplayTypes[i] == displayType)
                return false;
        }
        return true;
    };

    $scope.SetDisplayType = function (displayType) {
        if ($scope.IsSupported(displayType))
            $scope.csvData.DisplayType = displayType;
    };

    $scope.IsDisplayType = function (displayType) {
        return $scope.csvData.DisplayType == displayType;
    };

    $scope.csvData =
        {
            CsvDocument: "",//https://www.dropbox.com/s/dxeq4t9lrd9lzry/Mappe1.csv",

            //DisplayType: "Timeline",
            //ColumnsToIgnore:
            //    [
            //        { Name: "Projekt" }
            //    ],
            //HasColumnNames: true,
            //Width: 900,
            //GroupBy: "",
            //Timeline:
            //    { startDateColumn: "Deadline", endDateColumn: "" }
            //,
            //Columns:
            //[
            //    { Name: "Projekt", Type: "String" },
            //    { Name: "Tekst", Type: "String" },
            //    { Name: "Deadline", Type: "DateTime" }
            //]
        };
    
    $scope.inProgess = false;

    $scope.$watch($scope.csvData, function () { $scope.inProgess = false; $scope.trustSrc(); }, true)


    $scope.trustSrc = function () {
        //console.log("TrustSrc");
        if (!$scope.preview || $scope.inProgess)
            return false;

        $scope.inProgess = true;

        //$scope.preview = false;
        $http.post("/api/Csv/GetUrl", $scope.csvData)
            .success(function (data, status, headers, config) {
                var url = $sce.trustAsResourceUrl(data);
                $scope.iFrameTrustSrc = url;
                $scope.iFrameFullSrc = '<iframe src="' + $scope.iFrameTrustSrc + '" frameborder="0" style="float: left; width: ' + $scope.csvData.Width + 'px; height: 400px"></iframe>';
            }).error(function (data, status, headers, config) {
                alert("error");
                return status;
            });



        //var src = "http://" + window.location.host + "/" + $scope.csvData.DisplayType + ".aspx?csv=" + encodeURIComponent($scope.csvData.CsvDocument) +
        //    "&hasColumnNames=" + ($scope.csvData.HasColumnNames ? "1" : "0") +
        //    "&groupBy=" + $scope.csvData.GroupBy +
        //    "&width=" + $scope.csvData.Width +
        //    "&filterBy=" + $scope.csvData.FilterBy +
        //    "&filterValue=" + $scope.csvData.FilterValue;

        //if ($scope.csvData.DisplayType == "Timeline") {
        //    src += "&startDateColumn=" + $scope.csvData.Timeline.startDateColumn +
        //           "&endDateColumn=" + $scope.csvData.Timeline.endDateColumn +
        //           "&textColumn=" + $scope.csvData.Timeline.TextColumn;
        //}


        //console.log(src);
        //return $sce.trustAsResourceUrl(src);
    };


    $scope.preview = false;
    $scope.previewText = function () {
        if ($scope.preview)
            return "Stop preview";

        return "Preview";
    };

    $scope.changePreview = function () {
        $scope.preview = true;//!$scope.preview;
        $scope.inProgess = false;
        $scope.trustSrc();
    };

    $scope.getDateTimes = function () {
        var ret = [];

        angular.forEach($scope.csvData.Columns, function (col) {
            if (col.Type == "DateTime")
                ret.push(col);
        });
        return ret;
    };

    $scope.getStringCols = function () {
        var ret = [];

        angular.forEach($scope.csvData.Columns, function (col) {
            if (col.Type == "String")
                ret.push(col);
        });
        return ret;

    };

    $scope.isRSS = function () {
        return $scope.csvData.DisplayType === 'RSS';
    };
    
});