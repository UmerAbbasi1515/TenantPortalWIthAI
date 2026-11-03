//function renderBarChart(chartId, labels, data, options = {}) {
//    var chartElement = document.getElementById(chartId);

//    if (!chartElement) {
//        console.error(`Chart element with id ${chartId} not found.`);
//        return;
//    }
//    const defaultOptions = {
//        chart: {
//            type: 'bar',
//            height: options.height || 350
//        },
//        series: [{
//            name: options.label || 'Monthly Billing',
//            data: data
//        }],
//        xaxis: {
//            categories: labels,
//        },
//        plotOptions: {
//            bar: {
//                horizontal: options.horizontal || false,
//                dataLabels: {
//                    position: 'top',
//                }
//            },
//            colors: ['rgb(67, 185, 178)', 'rgb(252, 144, 92)', 'rgb(202, 146, 215)'],

//        },
//        dataLabels: {
//            enabled: options.datalabelsDisplay || true,
//            style: {
//                fontSize: options.datalabelsFontSize || '10px',
//                fontWeight: options.datalabelsFontWeight || 'bold',
//                colors: options.datalabelsColor || '#000'
//            }
//        },
//        fill: {
//            colors: options.backgroundColor || ['#660033']
//        },
//        stroke: {
//            colors: options.borderColor || ['#4BC0C0'],
//            width: options.borderWidth || 1
//        },
//        legend: {
//            position: options.legendPosition || 'bottom',
//            fontSize: options.legendFontSize || '10px'
//        },
//        grid: {
//            show: true,
//            borderColor: '#f1f1f1'
//        }
//    };

//    // Merge custom options if provided
//    const chartOptions = { ...defaultOptions, ...options };

//    const chart = new ApexCharts(chartElement, chartOptions);
//    chart.render();
//}


//function renderMultiBarChart(chartId, categories, datasets, options = {}) {
//    // Check if chart element exists
//    var chartElement = document.getElementById(chartId);
//    if (!chartElement) {
//        console.error(`Chart element with id ${chartId} not found.`);
//        return;
//    }

//    const defaultOptions = {
//        chart: {
//            type: 'bar', // Bar chart type
//            height: 350
//        },
//        series: datasets,
//        xaxis: {
//            categories: categories // Categories (e.g., months, years, etc.)
//        },
//        dataLabels: {
//            enabled: options.datalabelsDisplay || true, // Show data labels
//            style: {
//                fontSize: options.datalabelsFontSize || '10px',
//                fontWeight: options.datalabelsFontWeight || 'bold',
//                colors: options.datalabelsColor || ['#000000']
//            },
//            offsetY: -10, // Position data labels at the top of the bar
//            formatter: function (val, opts) {
//                return opts.w.globals.labels[opts.dataPointIndex] + ': ' + val; // Display label with value
//            }
//        },
//        tooltip: {
//            y: {
//                formatter: function (val) {
//                    return "$" + val + "k"; // Format tooltip value
//                }
//            }
//        },
//        legend: {
//            position: options.legendPosition || 'bottom', // Position of the legend
//            fontSize: options.legendFontSize || '10px'
//        },
//        plotOptions: {
//            bar: {
//                horizontal: options.horizontal || false, // Vertical bars (default)
//                columnWidth: options.columnWidth || '70%', // Custom width of bars
//                endingShape: options.endingShape || 'rounded' // Rounded end for bars
//            }
//        },
//        stacked: options.stacked || false, // Ensure bars are not stacked (if false)
//        beginAtZero: options.beginAtZero || true // Y-axis starts from 0
//    };

//    // Merge provided options with default options
//    const chartOptions = Object.assign({}, defaultOptions, options);

//    // Create and render the chart
//    const chart = new ApexCharts(chartElement, chartOptions);
//    chart.render();
//}


//function renderGroupedBarChart(chartId, labels, datasets, options = {}) {
//    var chartElement = document.getElementById(chartId);

//    if (!chartElement) {
//        console.error(`Chart element with id ${chartId} not found.`);
//        return;
//    }

//    // Prepare the series data from the datasets
//    const series = datasets.map(dataset => ({
//        name: dataset.label,
//        data: dataset.data
//    }));

//    // Default chart options for grouped bar chart
//    const defaultOptions = {
//        chart: {
//            type: 'bar',
//            height: options.height || 350
//        },
//        series: series,
//        xaxis: {
//            categories: labels, // Categories for X-axis (e.g., months)
//        },
//        plotOptions: {
//            bar: {
//                horizontal: false, // Set to false for vertical grouped bars
//                columnWidth: '45%', // Adjust width of each group bar
//                distributed: true, // Distribute the bars across categories
//                dataLabels: {
//                    position: 'top' // Position of the data labels
//                }
//            }
//        },
//        dataLabels: {
//            enabled: options.datalabelsDisplay || true, // Show data labels
//            style: {
//                fontSize: options.datalabelsFontSize || '10px',
//                fontWeight: options.datalabelsFontWeight || 'bold',
//                colors: options.datalabelsColor || '#000'
//            }
//        },
//        fill: {
//            opacity: 1
//        },
//        legend: {
//            position: options.legendPosition || 'bottom',
//            fontSize: options.legendFontSize || '10px'
//        },
//        grid: {
//            show: true,
//            borderColor: '#f1f1f1'
//        }
//    };

//    // Merge custom options with the default ones
//    const chartOptions = { ...defaultOptions, ...options };

//    // Create and render the chart
//    const chart = new ApexCharts(chartElement, chartOptions);
//    chart.render();
//}



//function renderPendingRenewalsChart(pendingRenewalsData) {
//    if (!pendingRenewalsData || !pendingRenewalsData.dataSet || pendingRenewalsData.dataSet.length === 0) {
//        document.getElementById("pendingRenewalsChart").innerHTML = '<div class="NotFound"><i class="fa fa-recycle"></i><strong>No Records Found.</strong></div>';
//        return;
//    }
//    var labels = pendingRenewalsData.dataSet.map(function (item) { return item.label; });
//    var sliceValues = pendingRenewalsData.dataSet.map(function (item) { return item.sliceValue; });
//    var percentageValues = pendingRenewalsData.dataSet.map(function (item) { return item.percentTage.toFixed(1); });

//    var options = {
//        animationEnabled: true,
//        innerRadius: "50%",
//        showInLegend: false,  // Optionally hide the legend
//        dataLabels: {
//            enabled: true,
//            style: {
//                fontSize: '12px',
//                fontWeight: 'bold',
//                colors: ['#FFFFFF']  // Color for percentage text
//            },
//            formatter: function (val, opts) {
//                var percentage = percentageValues[opts.seriesIndex];
//                return percentage + "%";  // Display only percentage
//            }
//        },
//    };

//    // Get the chart container
//    let chartContainer = document.getElementById("pendingRenewalsChart");

//    // Clear the previous chart content (if any)
//    while (chartContainer.firstChild) {
//        chartContainer.removeChild(chartContainer.firstChild);
//    }

//    // Define the series values for the chart
//    const series = sliceValues;

//    // Default chart options
//    const chartOptions = {
//        chart: {
//            type: 'donut',
//            height: options.height || 350,
//            toolbar: {
//                show: true,
//                tools: {
//                    download: true,
//                },
//            },
//        },
//        series: series,
//        plotOptions: {
//            pie: {
//                donut: {
//                    size: '80%' // Size of the donut hole
//                }
//            }
//        },
//        tooltip: {
//            shared: true,
//            y: {
//                formatter: function (val) {
//                    return val.toLocaleString(); // Show percentage in the tooltip
//                }
//            }
//        },
//        states: {
//            normal: {
//                filter: {
//                    type: 'none',
//                }
//            },
//            hover: {
//                filter: {
//                    type: 'darken',
//                    value: 0.4
//                }
//            },
//            active: {
//                allowMultipleDataPointsSelection: false,
//                filter: {
//                    type: 'darken',
//                    value: 0.5
//                }
//            }
//        },
//        colors: ['rgb(67, 185, 178)', 'rgb(252, 144, 92)'],
//        labels: labels,
//    };

//    // Create and render the new chart instance
//    const chart = new ApexCharts(chartContainer, chartOptions);
//    chart.render();
//}



//function renderNewLeasesChart(newLeasesChartData) {
//    debugger;
//    if (!newLeasesChartData || !newLeasesChartData.dataSet || newLeasesChartData.dataSet.length === 0) {
//        document.getElementById("newLeasesChart").innerHTML = '<div class="NotFound"><i class="fa fa-recycle"></i><strong>No Records Found.</strong></div>';
//        return;
//    }

//    // Extract labels, slice values, and percentages
//    var labels = newLeasesChartData.dataSet.map(function (item) { return item.label; });
//    var sliceValues = newLeasesChartData.dataSet.map(function (item) { return item.sliceValue; });
//    var percentages = newLeasesChartData.dataSet.map(function (item) { return item.percentTage; });

//    // Calculate total count
//    var totalCount = sliceValues.reduce(function (sum, value) {
//        return sum + value;
//    }, 0);

    
//    // Chart options
//    var options = {
//        series: sliceValues,
//        chart: {
//            type: 'donut',
//            height: 350,
//            toolbar: {
//                show: true,
//                tools: {
//                    download: true,
//                },
//            },
//        },
//        dataLabels: {
//            enabled: true,
//            formatter: function (val, opt) {
//                // Show percentage from the pre-calculated percentages array
//                return percentages[opt.seriesIndex].toFixed(1) + '%'; // Show percentage on each slice
//            },
//            style: {
//                fontSize: '12px',
//                fontWeight: 'bold',
//                colors: ['#FFFFFF']
//            }
//        },
//        tooltip: {
//            enabled: true,
//            y: {
//                formatter: function (val, opt) {
//                    return sliceValues[opt.seriesIndex].toLocaleString(); // Format tooltip with percentage
//                }
//            }
//        },
//        plotOptions: {
//            pie: {
//                donut: {
//                    size: '80%' // Adjust donut size
//                }
//            }
//        },
//        states: {
//            normal: {
//                filter: {
//                    type: 'none',
//                }
//            },
//            hover: {
//                filter: {
//                    type: 'darken',
//                    value: 0.4
//                }
//            },
//            active: {
//                allowMultipleDataPointsSelection: false,
//                filter: {
//                    type: 'darken',
//                    value: 0.5
//                }
//            }
//        },
//        colors: ['rgb(67, 185, 178)', 'rgb(252, 144, 92)', 'rgb(202, 146, 215)'],
//        labels: labels,
//        responsive: [{
//            breakpoint: 480,
//            options: {
//                chart: {
//                    width: 200
//                },
//                legend: {
//                    position: 'bottom'
//                }
//            }
//        }],
//        annotations: {
//            position: 'front',
//            text: `Total: ${totalCount}`,  // Directly set the total count in the center
//            style: {
//                fontSize: '20px',
//                fontWeight: 'bold',
//                color: '#333'
//            },
//            x: '50%',  // Center horizontally
//            y: '50%',  // Center vertically
//            textAnchor: 'middle'  // Ensure the text is centered
//        }
//    };

//    let chartContainer = document.getElementById("newLeasesChart");
//    while (chartContainer.firstChild) {
//        chartContainer.removeChild(chartContainer.firstChild);
//    }
//    const chart = new ApexCharts(chartContainer, options);
//    chart.render(); // Render the chart
//}


//function renderARAgingChart(ARAgingChartData) {

//    var labels = ARAgingChartData.dataSet.map(function (item) { return item.label; });;
//    var sliceValues = ARAgingChartData.dataSet.map(function (item) { return item.sliceValue; });
//    var options = {
//        legendPosition: 'bottom',
//        legendTextColor: 'black',
//        cutout: '80%'
//    };


//    const series = data;
//    const defaultOptions = {
//        chart: {
//            type: 'donut',
//            height: options.height || 350
//        },
//        series: series,
//        labels: labels,
//        plotOptions: {
//            pie: {
//                donut: {
//                    size: '80%'
//                }
//            }
//        },
//        dataLabels: {
//            enabled: true,
//            style: {
//                fontSize: '12px',
//                fontWeight: 'bold',
//                colors: ['#FFFFFF']
//            },
//            formatter: function (val, opt) {
//                return opt.w.globals.labels[opt.seriesIndex] + ': ' + val.toFixed(1) + '%'; // Label format with percentage
//            }
//        },
//        tooltip: {
//            shared: true,
//            y: {
//                formatter: function (val) {
//                    return val.toFixed(1) + '%';
//                }
//            }
//        },
//        legend: {
//            position: 'bottom',
//            fontSize: options.legendFontSize || '12px',
//            labels: {
//                colors: ['#000000']
//            }
//        },
//        colors: options.colors || ['#008FFB', '#00E396', '#FEB019', '#FF4560', '#775DD0', '#3B3C3D'],
//    };

//    const chartOptions = Object.assign({}, defaultOptions, options);
//    const chart = new ApexCharts("aRAgingDonutChart", chartOptions);
//    chart.render();
//}

//function renderLineChart(chartId, data, xAxisKey, yAxisKey, seriesKey, options = {}) {

//    console.log(data);

//    // Check if chart element exists
//    var chartElement = document.getElementById(chartId);
//    if (!chartElement) {
//        console.error(`Chart element with id ${chartId} not found.`);
//        return;
//    }

//    var options = {
//        series: [
//            {
//                name: "High - 2013",
//                data: [28, 29, 33, 36, 32, 32, 33] // Adjust these values based on your temperature data
//            },
//            {
//                name: "Low - 2013",
//                data: [12, 11, 14, 18, 17, 13, 13] // Adjust these values based on your temperature data
//            }
//        ],
//        chart: {
//            height: 350,
//            type: 'line',
//            dropShadow: {
//                enabled: true,
//                color: '#000',
//                top: 18,
//                left: 7,
//                blur: 10,
//                opacity: 0.5
//            },
//            zoom: {
//                enabled: false
//            },
//            toolbar: {
//                show: false
//            }
//        },
//        colors: ['#77B6EA', '#545454'],
//        dataLabels: {
//            enabled: true,
//        },
//        stroke: {
//            curve: 'smooth'
//        },
//        title: {
//            text: 'Average High & Low Temperature',
//            align: 'left'
//        },
//        grid: {
//            borderColor: '#e7e7e7',
//            row: {
//                colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
//                opacity: 0.5
//            },
//        },
//        markers: {
//            size: 1
//        },
//        xaxis: {
//            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'], // You can adjust these if you have more data points
//            title: {
//                text: 'Month'
//            }
//        },
//        yaxis: {
//            title: {
//                text: 'Temperature'
//            },
//            min: 5,
//            max: 40
//        },
//        legend: {
//            position: 'top',
//            horizontalAlign: 'right',
//            floating: true,
//            offsetY: -25,
//            offsetX: -5
//        }
//    };
//    const chart = new ApexCharts(chartElement, options);
//    chart.render();
//}


//// Function to render the Terminations chart
//function renderTerminationsChart(terminationsChartData) {
//    if (!terminationsChartData || !terminationsChartData.dataSet || terminationsChartData.dataSet.length === 0) {
//        document.getElementById("terminationsChart").innerHTML = '<div class="NotFound"><i class="fa fa-recycle"></i><strong>No Records Found.</strong></div>';
//        return;
//    }

//    // Map slice values and percentages from the data
//    var label = terminationsChartData.dataSet.map(function (item) { return item.label; });
//    var sliceValues = terminationsChartData.dataSet.map(function (item) { return item.sliceValue; });

//    // Directly use the pre-calculated percentages from the data
//    var percentages = terminationsChartData.dataSet.map(function (item) { return item.percentTage.toFixed(1); }); // Ensuring the percentage is a string with one decimal place

//    var chartElement = document.getElementById('terminationsChart');
//    if (!chartElement) {
//        console.error("Chart element with id 'terminationsChart' not found.");
//        return;
//    }

//    // Remove any existing chart content from the container
//    while (chartElement.firstChild) {
//        chartElement.removeChild(chartElement.firstChild);
//    }

//    // Set chart options
//    var options = {
//        series: sliceValues,
//        chart: {
//            type: 'donut',
//            height: 350,
//            toolbar: {
//                show: true,
//                tools: {
//                    download: true,
//                },
//            },
//        },
//        dataLabels: {
//            enabled: true,
//            formatter: function (val, opt) {
//                // Use the pre-calculated percentage value directly
//                return percentages[opt.seriesIndex] + '%'; // Show percentage on each slice
//            },
//            style: {
//                fontSize: '12px',
//                fontWeight: 'bold',
//                colors: ['#FFFFFF']  // White color for the percentage text
//            }
//        },
//        tooltip: {
//            enabled: true,
//            y: {
//                formatter: function (val, opt) {
//                    return sliceValues[opt.seriesIndex];
//                }
//            }
//        },
//        plotOptions: {
//            pie: {
//                donut: {
//                    size: '75%'
//                }
//            }
//        },
//        states: {
//            normal: {
//                filter: {
//                    type: 'none',
//                }
//            },
//            hover: {
//                filter: {
//                    type: 'darken',
//                    value: 0.4
//                }
//            },
//            active: {
//                allowMultipleDataPointsSelection: false,
//                filter: {
//                    type: 'darken',
//                    value: 0.5
//                }
//            }
//        },
//        colors: ['rgb(67, 185, 178)', 'rgb(252, 144, 92)', 'rgb(202, 146, 215)'],
//        labels: label,
//        responsive: [{
//            breakpoint: 480,
//            options: {
//                chart: {
//                    width: 200
//                },
//                legend: {
//                    position: 'bottom'
//                }
//            }
//        }]
//    };

//    // Destroy the old chart instance if it exists
//    const existingChart = chartElement.__apexcharts__;
//    if (existingChart) {
//        existingChart.destroy();
//    }

//    // Create and render the new chart instance
//    const terminationsChartInstance = new ApexCharts(chartElement, options);
//    terminationsChartInstance.render().then(function () {
//        console.log("Chart rendered successfully.");
//    }).catch(function (error) {
//        console.error("Error rendering chart:", error);
//    });
//}




////function renderOccupancybyTypeChart(renderOccupancybyTypeData) {
////    

////    if (!renderOccupancybyTypeData || renderOccupancybyTypeData.length === 0) {
////        document.getElementById("horizontalStackedBarChart").innerHTML =
////            '<div class="NotFound"><i class="fa fa-recycle"></i><strong>No Records Found.</strong></div>';
////        return;
////    }

////    let occupiedData = [];
////    let vacantData = [];
////    let unitTypes = [];

////    renderOccupancybyTypeData.forEach(data => {
////        unitTypes.push(data.UnitType);
////        occupiedData.push(data.OccupiedUnits);
////        vacantData.push(data.VacantUnits);
////    });

////    var options = {
////        series: [{
////            name: 'Marine Sprite',
////            data: occupiedData
////        }, {
////            name: 'Striking Calf',
////            data: vacantData
////        }, {
////            name: 'Tank Picture',
////            data: [12, 17, 11, 9, 15, 11, 20]
////        }, {
////            name: 'Bucket Slope',
////            data: [9, 7, 5, 8, 6, 9, 4]
////        }, {
////            name: 'Reborn Kid',
////            data: [25, 12, 19, 32, 25, 24, 10]
////        }],
////        chart: {
////            type: 'bar',
////            height: 350,
////            stacked: true,
////        },
////        plotOptions: {
////            bar: {
////                horizontal: true,
////                dataLabels: {
////                    total: {
////                        enabled: true,
////                        offsetX: 0,
////                        style: {
////                            fontSize: '13px',
////                            fontWeight: 900
////                        }
////                    }
////                }
////            },
////        },
////        stroke: {
////            width: 1,
////            colors: ['#fff']
////        },
////        title: {
////            text: 'Fiction Books Sales'
////        },
////        xaxis: {
////            categories: [2008, 2009, 2010, 2011, 2012, 2013, 2014],
////            labels: {
////                formatter: function (val) {
////                    return val + "K"
////                }
////            }
////        },
////        yaxis: {
////            title: {
////                text: undefined
////            },
////        },
////        tooltip: {
////            y: {
////                formatter: function (val) {
////                    return val + "K"
////                }
////            }
////        },
////        fill: {
////            opacity: 1
////        },
////        legend: {
////            position: 'top',
////            horizontalAlign: 'left',
////            offsetX: 40
////        }
////    };

////    let chartContainer = document.getElementById("horizontalStackedBarChart");

////    while (chartContainer.firstChild) {
////        chartContainer.removeChild(chartContainer.firstChild);
////    }

////    const chart = new ApexCharts(chartContainer, options);
////    chart.render();
////}

////function renderOccupancybyTypeChart(typeds) {
////    

////    let renderOccupancybyTypeData = [
////        { UnitType: 'Apartment/Flat', TotalUnits: 30, OccupiedUnits: 15, VacantUnits: 30 },
////        { UnitType: 'Duplex', TotalUnits: 47, OccupiedUnits: 30, VacantUnits: 60 },
////        { UnitType: 'Town House', TotalUnits: 1, OccupiedUnits: 0, VacantUnits: 1 },
////        { UnitType: 'Villa', TotalUnits: 1473, OccupiedUnits: 3, VacantUnits: 100 },
////        { UnitType: 'Retail Shop', TotalUnits: 60, OccupiedUnits: 50, VacantUnits: 100 },
////        { UnitType: 'Office', TotalUnits: 1063, OccupiedUnits: 2, VacantUnits: 400 },
////        { UnitType: 'Showroom', TotalUnits: 72, OccupiedUnits: 0, VacantUnits: 72 },
////        { UnitType: 'Warehouse', TotalUnits: 22, OccupiedUnits: 0, VacantUnits: 22 },
////        { UnitType: 'Labour Camp', TotalUnits: 26, OccupiedUnits: 0, VacantUnits: 26 }
////    ];

////    // Extract data for the chart
////    let unitTypes = typeds.map(item => item.UnitType);
////    let occupiedData = typeds.map(item => item.OccupiedUnits);
////    let vacantData = typeds.map(item => item.VacantUnits);
////    let totalData = typeds.map(item => item.TotalUnits);  // Total Units for each row

////    var options = {
////        series: [{
////            name: 'Vacant Units',
////            data: vacantData
////        },
////        {
////            name: 'Occupied Units',
////            data: occupiedData
////        }

////        ],
////        chart: {
////            type: 'bar',
////            height: 350,
////            stacked: true,  // Stacked bars to show both occupied and vacant in the same row
////        },
////        plotOptions: {
////            bar: {
////                horizontal: true,  // Horizontal bars
////                columnWidth: '70%',  // Reduces the width of the bars
////                barSpacing: 10,  // Adds space between stacked bars (effectively creates space between Occupied and Vacant bars)
////                minBarHeight: 15  // Set minimum bar height to ensure even small values are visible
////            },
////        },
////        stroke: {
////            width: 1,
////            colors: ['#fff']  // White stroke for better contrast
////        },
////        title: {
////            text: 'Unit Occupancy by Type'  // Chart title
////        },
////        xaxis: {
////            categories: unitTypes,
////            labels: {
////                show: false  // Hide X-axis labels (like 0, 200, etc.)
////            }
////        },
////        tooltip: {
////            y: {
////                formatter: function (val, opts) {
////                    // Show tooltip with both occupied, vacant, and total units
////                    let total = totalData[opts.dataPointIndex];
////                    return `${val} Units (Total: ${total} Units)`;  // Tooltip with total units info
////                }
////            }
////        },
////        fill: {
////            opacity: 1
////        },
////        legend: {
////            position: 'top',
////            horizontalAlign: 'left',
////            offsetX: 0
////        }
////    };

////    // Get the chart container
////    let chartContainer = document.getElementById("horizontalStackedBarChart");

////    // Remove any existing chart if it exists
////    while (chartContainer.firstChild) {
////        chartContainer.removeChild(chartContainer.firstChild);
////    }

////    // Create a new ApexCharts instance and render the chart
////    const chart = new ApexCharts(chartContainer, options);
////    chart.render();
////}



//function renderOccupancybyTypeChart(typeds) {


//    // Extract data for the chart
//    let unitTypes = typeds.map(item => item.unitType);
//    let occupiedData = typeds.map(item => item.occupiedUnits);
//    let vacantData = typeds.map(item => item.vacantUnits);

//    var options = {
//        chart: {
//            type: 'bar',
//            height: 350
//        },
//        series: [
//            {
//                name: 'Occupied Units',
//                data: occupiedData,
//                color: 'rgb(67, 185, 178)',  // Example color for Occupied Units
//                borderWidth: 1,
//                barThickness: 20,
//                categoryPercentage: 0.5,
//                barPercentage: 0.8
//            },
//            {
//                name: 'Vacant Units',
//                data: vacantData,
//                color: 'rgb(252, 144, 92)',  // Example color for Vacant Units
//                borderWidth: 1,
//                barThickness: 20,
//                categoryPercentage: 0.5,
//                barPercentage: 0.8
//            }
//        ],
//        xaxis: {
//            categories: unitTypes,
//        },
//        dataLabels: {
//            enabled: false,
//            style: {
//                fontSize: '10px',
//                fontWeight: 'bold',
//                colors: ['#000000']
//            },
//            offsetY: -10,
//            formatter: function (val) {
//                return val;
//            }
//        },
//        tooltip: {
//            y: {
//                formatter: function (val) {
//                    return val;
//                }
//            }
//        },
//        legend: {
//            position: 'bottom',
//            fontSize: '10px'
//        },
//        plotOptions: {
//            bar: {
//                horizontal: false,
//                columnWidth: '70%',
//                endingShape: 'rounded'
//            }
//        },
//        states: {
//            normal: {
//                filter: {
//                    type: 'none',
//                }
//            },
//            hover: {
//                filter: {
//                    type: 'darken',
//                    value: 0.4
//                }
//            },
//            active: {
//                allowMultipleDataPointsSelection: false,
//                filter: {
//                    type: 'darken',
//                    value: 0.5
//                }
//            }
//        },
//        stacked: false,  // For grouped bars
//        beginAtZero: true
//    };

//    // Get the chart container
//    let chartContainer = document.getElementById("horizontalStackedBarChart");

//    // Remove any existing chart if it exists
//    while (chartContainer.firstChild) {
//        chartContainer.removeChild(chartContainer.firstChild);
//    }

//    // Create a new ApexCharts instance and render the chart
//    const chart = new ApexCharts(chartContainer, options);
//    chart.render();
//}

//// Financial
//function renderBarChart(chartId, name, data) {
//    if (!data || data.length === 0) {
//        console.error("No data available for chart.");
//        return;
//    }

//    let categories = data.map(item => item.monthAndYear);
//    let chatData = data.map(item => item.totalAmount);

//    var options = {
//        chart: {
//            type: 'bar',
//            height: 350
//        },
        
//        states: {
//            normal: {
//                filter: {
//                    type: 'none',
//                }
//            },
//            hover: {
//                filter: {
//                    type: 'darken',
//                    value: 0.4
//                }
//            },
//            active: {
//                allowMultipleDataPointsSelection: false,
//                filter: {
//                    type: 'darken',
//                    value: 0.5
//                }
//            }
//        },

//        series: [
//            {
//                name: name,
//                data: chatData
//            }
//        ],

//        colors: ['rgb(67, 185, 178)', 'rgb(252, 144, 92)', 'rgb(202, 146, 215)'],
       
//        xaxis: {
//            categories: categories,
//            labels: {
//                style: {
//                    fontSize: '12px'
//                }
//            }
//        },
       
//        yaxis: {
//            labels: {
//                hideOverlappingLabels: true ,
//                style: {
//                    fontSize: '10px'
//                },
//                formatter: function (val) {
//                    if (val >= 1000000) return (val / 1000000).toFixed(1) + 'M';
//                    if (val >= 1000) return (val / 1000).toFixed(1) + 'K';
//                    return val;
//                }
//            }
//        },
//        plotOptions: {
//            bar: {
//                dataLabels: {
//                    position: 'bottom', // ✅ Keep labels inside the bars
//                    hideOverflowingLabels: false,
//                    colors : ['#ffffff'] // ✅ White for inside the bar
//                },
//            }
//        },
//        dataLabels: {
//            enabled: false,
//            style: {
//                fontSize: '11px',
//                fontWeight: 'bold',
//                colors: ['#ffffff'] // ✅ White for inside the bar
//            },
//            formatter: function (val) {
//                if (val >= 1000000) return (val / 1000000).toFixed(1) + 'M';
//                if (val >= 1000) return (val / 1000).toFixed(1) + 'K';
//                return val;
//            }
//        },
//        legend: {
//            show: false
//        },
//        grid: {
//            padding: {
//                top: 20,
//                right: 20,
//                bottom: 0,
//                left: 10
//            }
//        }
//    };

//    let chartContainer = document.getElementById(chartId);

//    if (!chartContainer) {
//        console.error("Chart container not found!");
//        return;
//    }

//    // Destroy any existing chart before creating a new one
//    if (chartContainer.chartInstance) {
//        chartContainer.chartInstance.destroy();
//    }

//    const chart = new ApexCharts(chartContainer, options);
//    chart.render();

//    chartContainer.chartInstance = chart;
//}

//function renderPieChart(chartId, name, data) {
    

//    var labels = data.dataSet.map(item => item.Label);
//    if (!data.dataSet || data.dataSet.length === 0) {
//        document.getElementById(chartId).innerHTML = '<div class="NotFound"><i class="fa fa-recycle"></i><strong>No Records Found.</strong></div>';
//        return;
//    }
    
//    var labels = data.dataSet.map(function (item) { return item.label; });
//    var sliceValues = data.dataSet.map(function (item) { return item.sliceValue; });
//    var percentageValues = data.dataSet.map(function (item) { return item.percentTage.toFixed(1); });

//    var options = {
//        animationEnabled: true,
//        innerRadius: "50%",
//        dataLabels: {
//            enabled: true,
//            style: {
//                fontSize: '12px',
//                fontWeight: 'bold',
//                colors: ['#FFFFFF']  // Color for percentage text
//            },
//            formatter: function (val, opts) {
//                var percentage = percentageValues[opts.seriesIndex];
//                return percentage + "%";  // Display only percentage
//            }
//        },
//        plugins: {
//            legend: {
//              labels: {
//                usePointStyle: false,  // Enables round/circular markers
//                pointStyle: 'circle', // Makes the legend symbol a circle
//                font: {
//                  size: 14
//                }
//              },
//              position: 'right' // Or 'bottom', 'top', 'left'
//            }
//          }
//    };

//    // Get the chart container
//    let chartContainer = document.getElementById(chartId);

//    // Clear the previous chart content (if any)
//    while (chartContainer.firstChild) {
//        chartContainer.removeChild(chartContainer.firstChild);
//    }

//    // Define the series values for the chart
//    const series = sliceValues;

//    // Default chart options
//    const chartOptions = {
//        chart: {
//            type: 'donut',
//            height: options.height || 350,
//            toolbar: {
//                show: true,
//                tools: {
//                    download: true,
//                },
//            },
//        },
//        series: series,
//        plotOptions: {
//            pie: {
//                donut: {
//                    size: '80%' // Size of the donut hole
//                }
//            }
            
//        },
//        tooltip: {
//            shared: true,
//            y: {
//                formatter: function (val) {
//                    return val.toLocaleString(); // Show percentage in the tooltip
//                }
//            }
//        },
//        states: {
//            normal: {
//                filter: {
//                    type: 'none',
//                }
//            },
//            hover: {
//                filter: {
//                    type: 'darken',
//                    value: 0.4
//                }
//            },
//            active: {
//                allowMultipleDataPointsSelection: false,
//                filter: {
//                    type: 'darken',
//                    value: 0.5
//                }
//            }
//        },
//       colors: ['rgb(67, 185, 178)', 'rgb(252, 144, 92)', 'rgb(202, 146, 215)'],

//        labels: labels,
//    };

//    // Create and render the new chart instance
//    const chart = new ApexCharts(chartContainer, chartOptions);
//    chart.render();
//}

