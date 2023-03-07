




//For Remove the Filter

/*function clearAll() {
    document.querySelectorAll("#filter-item").forEach(el => el.remove());
    document.querySelectorAll(".btnFilter1").forEach(el => el.remove());
}

function clearOne(id) {
    var a = document.getElementById(id);
    var b = a.parentElement;

    b.remove()
}
*/


//For filter-choices


var checkboxes = document.querySelectorAll(".checkbox");


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



// For Search The Mission

function myFunction() {
    var input, filter, cards, cardContainer, h4, title, i;
    input = document.getElementById("myFilter");
    filter = input.value.toUpperCase();
    cardContainer = document.getElementById("myItems");
    cards = cardContainer.getElementsByClassName("card");

    for (i = 0; i < cards.length; i++) {
        title = cards[i].querySelector(".card-body h4.card-title");
     
        if (title.innerText.toUpperCase().indexOf(filter) > -1) {
            cards[i].style.display = "";
        } else {
            cards[i].style.display = "none";
        }
    }
}

