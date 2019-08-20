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
  console.lo(facets);
  if (facets.xObject) {
    delete facets.xObject;
  }
  $('.facets-list-pre').jsonViewer(facets);
}

function handleModalClick(index) {
  document.querySelector('div.slide.active').classList.remove('active');
  document.querySelector('div.slide#s' + index).classList.add('active');
  $('#cardSelectionModal').modal('hide');
}

(function () {
  getVisitorDetails();
  const buttonDropdown = document.querySelector('button.dropdown');
  buttonDropdown.addEventListener('click', function(event) {
      $('#cardSelectionModal').modal('show');
  });
  const labelHeaders = document.querySelectorAll('div.slide > h2');
  for (let i = 0; i < labelHeaders.length; i += 1) {
    labelHeaders[i].addEventListener('click',
      function(event) {
        $('#cardSelectionModal').modal('show');
      });
  }
})();