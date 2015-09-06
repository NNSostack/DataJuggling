<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimelineJoggeling.ascx.cs" Inherits="UserControls_TimelineJoggeling" %>
<script type="text/javascript" src="https://www.google.com/jsapi?autoload={'modules':[{'name':'visualization',
       'version':'1','packages':['timeline']}]}"></script>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>


<style>
    svg {
        overflow: scroll!IMPORTANT;
    }

    .glyphicon {
        cursor: pointer; 
    }
</style>

<script type="text/javascript">

    var links = '[]';

    google.setOnLoadCallback(drawChart);

    <%# GetData() %>

    var showOnlyWithContent = <%# showOnlyWithContent.ToString().ToLower() %>;
    var startDate = new Date(<%# StartDate.Year %>,<%# StartDate.Month - 1 %>, <%# StartDate.Day %>);
    var endDate = new Date(<%# EndDate.Year %>,<%# EndDate.Month - 1 %>, <%# EndDate.Day %>);
    

    var timeSpan = new Date(endDate.getTime() - startDate.getTime());

    
    var zoomIndex = 0;
    var offset = 0;

    function getTimeLineObjects(startDate, endDate) {
        
        var clone = timeLineObjects();

        var rangeStart = startDate;
        var rangeEnd = endDate;

        for( i = 0; i < clone.length; i++)
        {
            if( clone[i][1] == "" )
            {
                clone[i][2] = startDate;
                clone[i][3] = clone[i][2];
            }
            
            var spanStart = clone[i][2];
            var spanEnd = clone[i][3];
            
            if( spanStart < startDate && spanEnd > startDate && spanEnd < endDate )
            {
                clone[i][2] = startDate;
            }

            else if( spanStart > startDate && spanStart < endDate && spanEnd >= endDate )
            {
                clone[i][3] = endDate;
            }

            else if( spanStart <= rangeStart && spanEnd >= rangeEnd)
            {
                clone[i][2] = rangeStart;
                clone[i][3] = rangeEnd;
            }




        }
        //console.log(startDate, endDate);
        var list = $.grep(clone, function (timeline, i) {
            return timeline[2] >= startDate && timeline[2] <= endDate || timeline[3] >= startDate && timeline[3] <= endDate && timeline[2] < timeline[3];

        });

        for( i = 0; i < list.length; i++)
        {
            if( list[i][1] == "" )
            {
                list[i][2] = new Date(startDate.getTime() - 1);
                list[i][3] = list[i][2];
            }
        }

        //console.log(new Date(startDate.getTime() - 1), new Date(startDate.getTime() - 1));
        //console.log(list);
        list.unshift([ 'Idag', '', new Date(startDate.getTime() - 1), new Date(startDate.getTime() - 1) ]);
        list.unshift([ 'Idag', '', endDate, endDate ]);

        return list;

    };

    function drawChart() {
        doDrawChart(startDate, endDate);
    };

    function doDrawChart(sDate, eDate)
    {
        var container = document.getElementById('chartTimeline');
        var chart = new google.visualization.Timeline(container);
        var dataTable = new google.visualization.DataTable();

        dataTable.addColumn({ type: 'string', id: 'Label' });
        dataTable.addColumn({ type: 'string', id: 'Deadline' });
        dataTable.addColumn({ type: 'date', id: 'Start' });
        dataTable.addColumn({ type: 'date', id: 'End' });
        
        dataTable.addRows(
            getTimeLineObjects(sDate, eDate)
        );

        var options = {
            timeline: { colorByRowLabel: true }
            , chartArea: { left: 0, top: 0, width: "100%", height: "100%" }
            , height: 5000
        };

        chart.draw(dataTable, options);
        var height = resize();

        options.height = height + 20;
        chart.draw(dataTable, options);

        var minX = 99999;
        var maxX = 0;


        var rects = $('rect');
        for (var i = 1; i < rects.length; i++) {
            
            //  Link to page
            //if( links.length > 0 )
            //{
            //    $(rects[i]).attr('data-index', i - 1);
            //    $(rects[i]).click(function () {
            //        console.log($(this).attr('data-index'));
            //        var url = getUrl($(this).attr('data-index'));
            //        window.location = url;
            //    });
            //}

            var x = parseInt($(rects[i]).attr('x'));
            if( minX > x && x > 0)
                minX = x;
            if( maxX < x && x > 0)
                maxX = x;

        }

        for (var i = 0; i < rects.length; i++) {
            var x = parseInt($(rects[i]).attr('x'));
            if( minX == x || maxX == x )
                rects[i].remove();
        }
    };

    function getUrl(index) {
        return links[index];
    };

    function resize() {
        $($('#chartTimeline').children()[0]).css("height", "");
        var svg = $($('#chartTimeline').find('svg')[0]);
        var g = $(svg.find('g')[0]);
        var rect = g.find('rect').last();
        
        var height = parseInt(rect.attr('height')) + 40; 
        
        svg.height(height);
        return height; 
    };

    function Zoom(index, off)
    {
        zoomIndex = zoomIndex + index;
        offset = offset + off;
        var newTimeSpan = (timeSpan / 20) * zoomIndex;
        var ts = (timeSpan / 20);
        var newOffset = offset * ts;
        
        
        doDrawChart(new Date(startDate.getTime() + newOffset + newTimeSpan), new Date(endDate.getTime() + newOffset - newTimeSpan));
    }


    <%# Links %>




</script>

<div <%# Width %>">
    <div style="text-align:center;">
        <span onclick="Zoom(0, -1);return false;" class="glyphicon glyphicon-chevron-left"></span>

        <span onclick="Zoom(1, 0);return false;" class="glyphicon glyphicon-zoom-in"></span>
        <span onclick="Zoom(-1, 0);return false;" class="glyphicon glyphicon-zoom-out"></span>

    
        <span onclick="Zoom(0, 1);return false;" class="glyphicon glyphicon-chevron-right"></span>
    </div>
</div>

<div id="chartTimeline"<%# Width %>></div>



