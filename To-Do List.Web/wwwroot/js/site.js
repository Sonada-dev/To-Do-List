// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function applyFilters() {
    var statusFilter = document.getElementById("statusFilter").value;
    var priorityFilter = document.getElementById("priorityFilter").value;

    var taskList = document.getElementById("taskList").children;

    for (var i = 0; i < taskList.length; i++) {
        var taskStatus = taskList[i].querySelector('.list-group-item:nth-child(3) span').textContent;
        var taskPriority = taskList[i].querySelector('.list-group-item:nth-child(2) span').textContent;

        var statusMatch = !statusFilter || taskStatus === statusFilter;
        var priorityMatch = !priorityFilter || taskPriority === priorityFilter;

        if (statusMatch && priorityMatch) {
            taskList[i].style.display = 'block';
        } else {
            taskList[i].style.display = 'none';
        }
    }
}
