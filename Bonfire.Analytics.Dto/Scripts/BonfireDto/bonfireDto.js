function getVisitorDetails() {
  $('.contact-list-pre').jsonViewer(contacts);
  $('.pages-list-pre').jsonViewer(pagesViewed);
  var goals = {};
  goals.CurrentGoals = goalsList;
  goals.PastGoals = pastGoals;
  $('.goals-list-pre').jsonViewer(goals);
  $('.visit-data-list-pre').jsonViewer(interactions);
  $('.engagement-plans-list-pre').jsonViewer(engagementPlanStates);
  var campaigns = {};
  campaigns.CurrentCampaign = currentCampaign;
  campaigns.PastCampaigns = pastCampaigns;
  $('.campaigns-list-pre').jsonViewer(campaigns);
  var profiles = {};
  profiles.CurrentProfiles = currentProfiles;
  profiles.PastProfiles = pastProfiles;
  $('.profiles-list-pre').jsonViewer(profiles);
  $('.facets-list-pre').jsonViewer(facets);
}

function fillObject(parent, object) {
  //      parent.innerHTML = JSON.stringify(object, undefined, 2);
  if (!object)
    return;
  renderProperties(parent, object);
}

function renderProperties(parent, object) {
  for (var key in object) {
    const newProperty = document.createElement('li');
    if (typeof object[key] === 'object') {
      if (!isNaN(key)) {
        newProperty.classList.add('array-li');
      }
      newProperty.innerHTML = !isNaN(key) ? '[' : key;
      renderSubProperties(newProperty, object[key]);
    } else {
      newProperty.innerHTML = key + ': ' + (object[key] || '');
    }
    parent.appendChild(newProperty);
  }
}

function renderSubProperties(parent, object) {
  for (var key in object) {
    const subPropertyList = document.createElement('ul');
    const subProperty = document.createElement('li');
    if (typeof object[key] === 'object') {
      if (!isNaN(key)) {
        subProperty.classList.add('array-li');
      }
      subProperty.innerHTML = !isNaN(key) ? '[' : key;
    } else {
      subProperty.innerHTML = key + ': ' + (object[key] || '');
    }
    subPropertyList.appendChild(subProperty);
    parent.appendChild(subPropertyList);
    if (typeof object[key] === 'object')
      renderSubProperties(subPropertyList, object[key]);
  }
}

function handleNextClick(e) {
  console.log(e);
  const total = document.querySelectorAll('input[type="radio"]');
  const totalLength = total.length;
  const current = document.querySelector('input[type="radio"]:checked');
  const currentId = Number(current.id.substring(1));
  console.log('clicked: ', currentId);
  let next = -1;
  if (currentId === totalLength) next = 1;
  else next = currentId + 1;
  console.log(currentId, '->', next)
  document.querySelector('input#s' + next).click();
  document.querySelector('button.prev').classList.add('fade-out');
  setTimeout(function () {
    document.querySelector('button.prev').classList.remove('fade-out');
  }, 200);
}

function handlePrevClick(e) {
  // console.log(e);
  const total = document.querySelectorAll('input[type="radio"]');
  const totalLength = total.length;
  const current = document.querySelector('input[type="radio"]:checked');
  const currentId = Number(current.id.substring(1));
  // console.log('clicked: ', currentId);
  let next = -1;
  if (currentId === 1) next = totalLength;
  else next = currentId - 1;
  // console.log(next, '<-', currentId)
  document.querySelector('input#s' + next).click();
  document.querySelector('button.next').classList.add('fade-out');
  setTimeout(function () {
    document.querySelector('button.next').classList.remove('fade-out');
  }, 200);
}

function handleModalClick(index) {
  document.querySelector('input#s' + index).click();
  $('#cardSelectionModal').modal('hide');
}

(function () {
  getVisitorDetails();
  const buttonNext = document.querySelector('button.next');
  buttonNext.addEventListener('click', function (event) {
    handleNextClick(event);
  });
  const buttonPrev = document.querySelector('button.prev');
  buttonPrev.addEventListener('click', function (event) {
    handlePrevClick(event);
  });
  const labelHeaders = document.querySelectorAll('label > h2');
  for (let i = 0; i < labelHeaders.length; i += 1) {
    labelHeaders[i].addEventListener('click', function (event) {
      $('#cardSelectionModal').modal('show');
    })
  }

  const labels = document.querySelectorAll('.content-mobile label');
  for (let i = 0; i < labels.length; i += 1) {
    let hammerElem = new Hammer(labels[i]);
    hammerElem.on("swipeleft", function (ev) {
      handleNextClick(ev);
    });
    hammerElem.on("swiperight", function (ev) {
      handlePrevClick(ev);
    });
  }
})();