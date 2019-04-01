
var app = angular.module("app", ['ngRoute']);

// =====================================
// configure the route navigation
// =====================================
app.config(function ($routeProvider) {
    $routeProvider
    .when('/',
    {
        templateUrl: 'Home.html',
        controller: 'HomeController'
    })
    .when('/About',
    {
        templateUrl: 'About.html',
        controller: 'AboutController'
    })
    .when('/Contact',
    {
        templateUrl: 'Contact.html',
        controller: 'ContactController'
    })
    .when('/PD',
    {
        templateUrl: '/SPA/PD/Index.html',
        controller: "PDController"
    })
    .when('/PD/Create',
    {
        templateUrl: '/SPA/PD/Create.html',
        controller: "PDControllerCreate"
    })
    .when('/PD/Edit/:id',
    {
        templateUrl: '/SPA/PD/Edit.html',
        controller: "PDControllerEdit"
    })
    .when('/PD/Details/:id',
    {
        templateUrl: '/SPA/PD/Details.html',
        controller: "PDControllerDetails"
    })
    .when('/PD/Delete/:id',
    {
        templateUrl: '/SPA/PD/Delete.html',
        controller: "PDControllerDelete"
    })
});

// ===================================================
// Angular Factory to create service to peform CRUD
// ===================================================
app.factory("PDService", function ($http) {
    var thisPDService = {};

    // get all data from database
    thisPDService.GetAll = function () {
        var promise = $http({
            method: 'GET',
            url: '/api/PersonalDetails'
        })
            .then(function (response) {
                return response.data;
            },
            function (response) {
                return response.data;
            });
        return promise;
    };


    // get single record from database
    thisPDService.GetSingle = function (id) {

        var promise = $http({
            method: 'GET',
            url: '/api/PersonalDetails/' + id
        })
            .then(function (response) {
                return response.data;
            },
            function (response) {
                return response.data;
            });
        return promise;
    };


    // post the data from database
    thisPDService.Insert = function (firstName, lastName, age, active) {
        var personalDetail = {
            FirstName: firstName,
            LastName: lastName,
            Age: age,
            Active: active,
        };

        var promise = $http({
            method: 'POST',
            url: '/api/PersonalDetails',
            data: personalDetail
        })
        .then(function (response) {
            return response.statusText;
        },
        function (response) {
            return response.statusText;
        });

        return promise;
    };

    // put the data from database
    thisPDService.Update = function (autoId, firstName, lastName, age, active) {
        var personalDetail = {
            AutoId: autoId,
            FirstName: firstName,
            LastName: lastName,
            Age: age,
            Active: active,
        };

        var promise = $http({
            method: 'PUT',
            url: '/api/PersonalDetails/' + autoId,
            data: personalDetail
        })
        .then(function (response) {
            return "Updated";
            // return response.statusText + ' ' + response.status + ' ' + response.data;
        },
        function (response) {
            return response.statusText + ' ' + response.status + ' ' + response.data;
        });

        return promise;
    };

    // delete the data from database
    thisPDService.Remove = function (autoId) {
        var promise = $http({
            method: 'DELETE',
            url: '/api/PersonalDetails/' + autoId
        })
        .then(function (response) {
            // return "Deleted";
            return response.statusText + ' ' + response.status + ' ' + response.data;
        },
        function (response) {
            return response.statusText + ' ' + response.status + ' ' + response.data;
        });

        return promise;
    };

    return thisPDService;
});

// ===================================================
// Create other controllers for respective pages
// ===================================================

// default controller
app.controller("SPAController", function ($scope, $rootScope) {
    $scope.Title = "Welcome";
    $rootScope.loading = false;
});

// Home controller
app.controller("HomeController", function ($scope) {
    $scope.Title = "Single Page Application (SPA)";
});

// About controller
app.controller("AboutController", function ($scope) {
    $scope.Title = "About us";
});

// Contact controller
app.controller("ContactController", function ($scope) {
    $scope.Title = "Contact us";
});