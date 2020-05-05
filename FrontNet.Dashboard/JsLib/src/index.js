import { Chart } from 'chart.js';
import 'chartjs-plugin-labels';

const defaultColors = [
    '#3366CC', '#DC3912', '#FF9900', '#109618', '#990099', '#3B3EAC', '#0099C6',
    '#DD4477', '#66AA00', '#B82E2E', '#316395', '#994499', '#22AA99', '#AAAA11',
    '#6633CC', '#E67300', '#8B0707', '#329262', '#5574A6', '#3B3EAC'
];

const getColorsSet = () => defaultColors;

const chartsRegistry = {};

const createChart = (canvasSelector, type, { title = '', data = [], labels = [] }) => {
    const canvas = document.querySelector(canvasSelector).getContext('2d');

    const chart = new Chart(canvas, {
        type,
        data: {
            labels,
            datasets: [{
                backgroundColor: getColorsSet(),
                data
            }]
        },
        options: {
            title: {
                display: !!title,
                text: title
            },
            responsive: true,
            legend: {
                position: 'right'
            },
            plugins: {
                labels: {
                    render: 'value',
                    fontColor: 'white',
                    fontSize: 18,
                    fontStyle: 'bold'
                }
            }
        }
    });

    chartsRegistry[canvasSelector] = chart;
};

const updateChart = (canvasSelector, data = []) => {
    const chart = chartsRegistry[canvasSelector];

    if (chart && chart.data && chart.data.datasets[0]) {
        chart.data.datasets[0].data = data;
        chart.update();
    }
};

export const charts = { createChart, updateChart };