/// Response models for the Fortune Teller API.

/// API Response wrapper.
class FortunetellerResponse {
  final String status;
  final dynamic error;
  final FortunetellerData? data;

  FortunetellerResponse({
    required this.status,
    this.error,
    this.data,
  });

  factory FortunetellerResponse.fromJson(Map<String, dynamic> json) => FortunetellerResponse(
    status: json['status'] as String? ?? '',
    error: json['error'],
    data: json['data'] != null ? FortunetellerData.fromJson(json['data']) : null,
  );

  Map<String, dynamic> toJson() => {
    'status': status,
    if (error != null) 'error': error,
    if (data != null) 'data': data,
  };
}

/// Response data for the Fortune Teller API.

class FortunetellerData {
  String? fortune;
  String? insight;
  String? timeframe;
  String? category;
  dynamic question;
  List<int>? luckyNumbers;
  String? luckyElement;
  String? luckyColor;
  String? luckyDay;
  String? timestamp;

  FortunetellerData({
    this.fortune,
    this.insight,
    this.timeframe,
    this.category,
    this.question,
    this.luckyNumbers,
    this.luckyElement,
    this.luckyColor,
    this.luckyDay,
    this.timestamp,
  });

  factory FortunetellerData.fromJson(Map<String, dynamic> json) => FortunetellerData(
      fortune: json['fortune'],
      insight: json['insight'],
      timeframe: json['timeframe'],
      category: json['category'],
      question: json['question'],
      luckyNumbers: (json['luckyNumbers'] as List?)?.cast<int>(),
      luckyElement: json['luckyElement'],
      luckyColor: json['luckyColor'],
      luckyDay: json['luckyDay'],
      timestamp: json['timestamp'],
    );
}

class FortunetellerRequest {
  String? category;
  String? question;

  FortunetellerRequest({
    this.category,
    this.question,
  });

  Map<String, dynamic> toJson() => {
      if (category != null) 'category': category,
      if (question != null) 'question': question,
    };
}
