declare module '@apiverve/fortuneteller' {
  export interface fortunetellerOptions {
    api_key: string;
    secure?: boolean;
  }

  export interface fortunetellerResponse {
    status: string;
    error: string | null;
    data: FortuneTellerData;
    code?: number;
  }


  interface FortuneTellerData {
      fortune:      string;
      insight:      string;
      timeframe:    string;
      category:     string;
      question:     null;
      luckyNumbers: number[];
      luckyElement: string;
      luckyColor:   string;
      luckyDay:     string;
      timestamp:    Date;
  }

  export default class fortunetellerWrapper {
    constructor(options: fortunetellerOptions);

    execute(callback: (error: any, data: fortunetellerResponse | null) => void): Promise<fortunetellerResponse>;
    execute(query: Record<string, any>, callback: (error: any, data: fortunetellerResponse | null) => void): Promise<fortunetellerResponse>;
    execute(query?: Record<string, any>): Promise<fortunetellerResponse>;
  }
}
