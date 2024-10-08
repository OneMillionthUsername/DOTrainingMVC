﻿
function extractValuesAndTextNodes() {
	// Das übergeordnete Element auswählen
	var parentElement = document.getElementById("solutionText");

	// Funktion zur rekursiven Extraktion von Textknoten und Werten
	function extractTextAndValues(node) {
		var result = '';

		// Wenn es ein Elementknoten ist, rufe den Wert des Elements (falls vorhanden) ab
		if (node.nodeType === Node.ELEMENT_NODE) {
			// Überspringe den ersten Knoten
			if (node.id !== "solutionText") {
				var elementValue = node.value || node.textContent;

				if (elementValue) {
					result += elementValue;
				}
			}

			// Durchlaufe alle Kinder des Elements
			for (var i = 0; i < node.childNodes.length; i++) {
				result += extractTextAndValues(node.childNodes[i]);
			}
		}

		return result;
	}

	var extractedContent = extractTextAndValues(parentElement);
	var solutionString = document.getElementById("solutionString");
	if (solutionString) {
		solutionString.value = extractedContent;
	}
}

function validateForm(solution) {
	//init vars
	//var solution = ['select', 'customers', ...];
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
		extractValuesAndTextNodes();
		return true; // Formular wird abgesendet, wenn die Validierung erfolgreich ist
	}
	return false;
}

document.addEventListener('DOMContentLoaded', function () {
	// Hier das erste Eingabefeld automatisch fokussieren
	var firstInput = document.getElementById('param0');
	if (firstInput) {
		firstInput.focus();
	}
});

