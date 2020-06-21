const uri = 'api/Employees';
let employees = [];

function getEmployees() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayEmployees(data))
        .catch(error => console.error('Unable to get employees.', error));
}

function addEmployee() {
    const addFirstNameTextbox = document.getElementById('add-firstName');
    const addLastNameTextbox = document.getElementById('add-lastName');
    const addJobPositionTextbox = document.getElementById('add-jobPosition');
    const addSalaryTextbox = document.getElementById('add-salary');

    const employee = {
        firstName: addFirstNameTextbox.value.trim(),
        lastName: addLastNameTextbox.value.trim(),
        jobPosition: addJobPositionTextbox.value.trim(),
        salary: addSalaryTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(employee)
    })
        .then(response => response.json())
        .then(() => {
            getEmployees();
            addFirstNameTextbox.value = '';
            addLastNameTextbox.value = '';
            addJobPositionTextbox.value = '';
            addSalaryTextbox.value = '';
        })
        .catch(error => console.error('Unable to add employee.', error));
}

function deleteEmployee(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getEmployees())
        .catch(error => console.error('Unable to delete employee.', error));
}

function displayEditForm(id) {
    const employee = employees.find(employee => employee.id === id);

    document.getElementById('edit-id').value = employee.id;
    document.getElementById('edit-firstName').value = employee.firstName;
    document.getElementById('edit-lastName').value = employee.lastName;
    document.getElementById('edit-jobPosition').value = employee.jobPosition;
    document.getElementById('edit-salary').value = employee.salary;
    document.getElementById('editForm').style.display = 'block';
}

function updateEmployee() {
    const employeeId = document.getElementById('edit-id').value;
    const employee = {
        id: parseInt(employeeId, 10),
        firstName: document.getElementById('edit-firstName').value.trim(),
        lastName: document.getElementById('edit-lastName').value.trim(),
        jobPosition: document.getElementById('edit-jobPosition').value.trim(),
        salary: document.getElementById('edit-salary').value.trim()
    };

    fetch(`${uri}/${employeeId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(employee)
    })
        .then(() => getEmployees())
        .catch(error => console.error('Unable to update employee.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = '';
}

function _displayCount(employeeCount) {
    const name = (employeeCount === 1) ? 'empleat' : 'empleats';

    document.getElementById('counter').innerText = `${employeeCount} ${name}`;
}

function _displayEmployees(data) {
    const tBody = document.getElementById('employees');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(employee => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Editar';
        editButton.setAttribute('onclick', `displayEditForm(${employee.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Eliminar';
        deleteButton.setAttribute('onclick', `deleteEmployee(${employee.id})`);

        let tr = tBody.insertRow();
        
        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(employee.firstName);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode1 = document.createTextNode(employee.lastName);
        td2.appendChild(textNode1);

        let td3 = tr.insertCell(2);
        let textNode2 = document.createTextNode(employee.jobPosition);
        td3.appendChild(textNode2);

        let td4 = tr.insertCell(3);
        let textNode3 = document.createTextNode(employee.salary);
        td4.appendChild(textNode3);

        let td5 = tr.insertCell(4);
        td5.appendChild(editButton);

        let td6 = tr.insertCell(5);
        td6.appendChild(deleteButton);
    });

    employees = data;
}