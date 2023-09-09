// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function validateForm(solution) {
	//init vars
	//var solution = ['select', 'customers'];
	var valueContainer = {};
	var fieldContainer = {};
	var result = true;

	//create var names/key for values and fields
	for (var i = 0; i < solution.length; i++) {
		var valueName = "param" + i;
		var fieldName = "field" + i;
		valueContainer[valueName] = document.getElementById(valueName).value; //get value
		fieldContainer[fieldName] = document.getElementById(valueName); //get input field
	}

	for (var i = 0; i < solution.length; i++) {
		if (valueContainer["param" + i].toLowerCase().trim() !== solution[i].toLowerCase()) {
			fieldContainer["field" + i].style.backgroundColor = "red";
			result = false; // Formular wird nicht abgesendet
		}
		else {
			fieldContainer["field" + i].style.backgroundColor = "green";
		}
	}
	
	if (result) {
		return true; // Formular wird abgesendet, wenn die Validierung erfolgreich ist
	}
	return false;
}

// Event-Listener, der auf das DOMContentLoaded-Ereignis wartet
document.addEventListener('DOMContentLoaded', function () {
	// Hier das erste Eingabefeld automatisch fokussieren
	var firstInput = document.getElementById('param0');
	if (firstInput) {
		firstInput.focus();
	}
});

