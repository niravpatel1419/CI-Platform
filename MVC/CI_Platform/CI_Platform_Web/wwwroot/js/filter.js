//For Filtering the Mission

function countryBtn(id) {
    let url = window.location.href;
    const urlParts = url.split('?');

    if (urlParts.length >= 2) {
        const baseUrl = urlParts.shift();
        const queryString = urlParts.join('?');
        const params = new URLSearchParams(queryString);

        const countries = params.getAll('ACountries').filter(country => country !== id);

        if (countries.length > 0) {
            params.delete('ACountries');
            countries.forEach(country => params.append('ACountries', country));
        } else {
            params.delete('ACountries');
        }

        const updatedParams = params.toString();
        if (updatedParams) {
            url = `${baseUrl}?${updatedParams}`;
        } else {
            url = baseUrl;
        }

        window.location.href = url;
    }
}
function cityBtn(id) {
    let url = window.location.href;
    const urlParts = url.split('?');

    if (urlParts.length >= 2) {
        const baseUrl = urlParts.shift();
        const queryString = urlParts.join('?');
        const params = new URLSearchParams(queryString);

        const countries = params.getAll('ACities').filter(country => country !== id);

        if (countries.length > 0) {
            params.delete('ACities');
            countries.forEach(country => params.append('ACities', country));
        } else {
            params.delete('ACities');
        }

        const updatedParams = params.toString();
        if (updatedParams) {
            url = `${baseUrl}?${updatedParams}`;
        } else {
            url = baseUrl;
        }

        window.location.href = url;
    }
}
function themeBtn(id) {
    let url = window.location.href;
    const urlParts = url.split('?');

    if (urlParts.length >= 2) {
        const baseUrl = urlParts.shift();
        const queryString = urlParts.join('?');
        const params = new URLSearchParams(queryString);

        const countries = params.getAll('ATheme').filter(country => country !== id);

        if (countries.length > 0) {
            params.delete('ATheme');
            countries.forEach(country => params.append('ATheme', country));
        } else {
            params.delete('ATheme');
        }

        const updatedParams = params.toString();
        if (updatedParams) {
            url = `${baseUrl}?${updatedParams}`;
        } else {
            url = baseUrl;
        }

        window.location.href = url;
    }
}
function clearall() {
    var url = window.location.href
    var mainurl = url.split("?")[0];
    window.location.href = mainurl;

}
function updateUrlCountry() {
    const country = Array.from(document.querySelectorAll('input[name="country"]:checked')).map(el => el.id);
    let url = window.location.href;

    let separator = url.indexOf('?') !== -1 ? '&' : '?';
    url += separator + 'ACountries=' + country;
    window.location.href = url;

}

function updateUrlCities() {
    const city = Array.from(document.querySelectorAll('input[name="city"]:checked')).map(el => el.id);
    let url = window.location.href;
    let separator = url.indexOf('?') !== -1 ? '&' : '?';
    url += separator + 'ACities=' + city;
    window.location.href = url;


}
function updateUrlTheme() {
    const theme = Array.from(document.querySelectorAll('input[name="theme"]:checked')).map(el => el.id);
    let url = window.location.href;
    let separator = url.indexOf('?') !== -1 ? '&' : '?';
    url += separator + 'ATheme=' + theme;
    alert("ahdsh")

    window.location.href = url;


}







































//For filter-choices


/*var checkboxes = document.querySelectorAll(".checkbox");


let filtersSection = document.querySelector(".filterSection");

var listArray = [];

var filterList = document.querySelector(".filter-list");

var len = listArray.length;

for (var checkbox of checkboxes) {
    checkbox.addEventListener("click", function () {
        if (this.checked == true) {
            addElement(this, this.value);
        }
        else {
            removeElement(this.value);
            console.log("unchecked");
        }
    })
}


function addElement(current, value) {
    let filtersSection = document.querySelector(".filterSection");



    let createdTag = document.createElement('span');
    createdTag.classList.add('filter-list');
    createdTag.classList.add('ps-3')
    createdTag.classList.add('pe-2');
    createdTag.classList.add('me-2');
    createdTag.innerHTML = value;
    createdTag.setAttribute('id', value);
    createdTag.setAttribute('name', value);

    let crossButton = document.createElement('button');
    crossButton.classList.add("filter-close-button");

    let cross = '&times;'



    crossButton.addEventListener('click', function () {
        let elementToBeRemoved = document.getElementById(value);

        console.log(elementToBeRemoved);
        console.log(current);
        elementToBeRemoved.remove();

        current.checked = false;
    })

    crossButton.innerHTML = cross;


    // let crossButton = '&times;'

    createdTag.appendChild(crossButton);
    filtersSection.appendChild(createdTag);

}

function removeElement(value) {

    let filtersSection = document.querySelector(".filterSection");

    let elementToBeRemoved = document.getElementById(value);
    filtersSection.removeChild(elementToBeRemoved);

}
*/