

function extractValuesAndTextNodes() {
	// Das übergeordnete Element auswählen
	var parentElement = document.getElementById("solutionText");

	// Funktion zur rekursiven Extraktion von Textknoten und Werten
	function extractTextAndValues(node) {
		var result = '';

		//if (node.nodeType === Node.TEXT_NODE) {
		//	// Wenn es ein Textknoten ist, füge den Text hinzu
		//	result += node.textContent.trim();
		//} else 
		if (node.nodeType === Node.ELEMENT_NODE) {
			// Wenn es ein Elementknoten ist, rufe den Wert des Elements (falls vorhanden) ab
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

	// Text und Werte aus dem übergeordneten Element extrahieren
	var extractedContent = extractTextAndValues(parentElement);

	// Den gesamten Text und Werte anzeigen oder anderweitig verwenden
	console.log(extractedContent);

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

// Event-Listener, der auf das DOMContentLoaded-Ereignis wartet
document.addEventListener('DOMContentLoaded', function () {
	// Hier das erste Eingabefeld automatisch fokussieren
	var firstInput = document.getElementById('param0');
	if (firstInput) {
		firstInput.focus();
	}
});

