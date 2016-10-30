
scores = [1,2,3,4,5];
for (var score in scores){
	/*
	$.get('/highestScores', score.serialize(), function(data) {
		    console.log("first:",data.first);
		    console.log("second:",data.second);
		    console.log("third:",data.third);
	});
	*/

	//var myJSONData = '{"newScore":' + score.toString() + '}';

    $.get("/highestScores", function(data, status){
    	console.log("first:",data.first);
		console.log("second:",data.second);
		console.log("third:",data.third);
    });

    $.post("/updateScores",
    {
        newScore: score.toString()
    },
    function(data, status){
        alert("Data: " + data + "\nStatus: " + status);
    });

    /*
	$.ajax({
            type: 'GET',
            url: '/highestScores',
            data: myJSONData,
            dataType: 'application/json',
            success: function(data) { 
	            console.log("first:",data.first);
			    console.log("second:",data.second);
		    	console.log("third:",data.third);
            } // Success Function 
	});
	*/
}


