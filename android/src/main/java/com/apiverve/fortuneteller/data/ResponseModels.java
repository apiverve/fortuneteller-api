// Converter.java

// To use this code, add the following Maven dependency to your project:
//
//
//     com.fasterxml.jackson.core     : jackson-databind          : 2.9.0
//     com.fasterxml.jackson.datatype : jackson-datatype-jsr310   : 2.9.0
//
// Import this package:
//
//     import com.apiverve.data.Converter;
//
// Then you can deserialize a JSON string with
//
//     FortuneTellerData data = Converter.fromJsonString(jsonString);

package com.apiverve.fortuneteller.data;

import java.io.IOException;
import com.fasterxml.jackson.databind.*;
import com.fasterxml.jackson.databind.module.SimpleModule;
import com.fasterxml.jackson.core.JsonParser;
import com.fasterxml.jackson.core.JsonProcessingException;
import java.util.*;
import java.time.LocalDate;
import java.time.OffsetDateTime;
import java.time.OffsetTime;
import java.time.ZoneOffset;
import java.time.ZonedDateTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeFormatterBuilder;
import java.time.temporal.ChronoField;

public class Converter {
    // Date-time helpers

    private static final DateTimeFormatter DATE_TIME_FORMATTER = new DateTimeFormatterBuilder()
            .appendOptional(DateTimeFormatter.ISO_DATE_TIME)
            .appendOptional(DateTimeFormatter.ISO_OFFSET_DATE_TIME)
            .appendOptional(DateTimeFormatter.ISO_INSTANT)
            .appendOptional(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss.SX"))
            .appendOptional(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ssX"))
            .appendOptional(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"))
            .toFormatter()
            .withZone(ZoneOffset.UTC);

    public static OffsetDateTime parseDateTimeString(String str) {
        return ZonedDateTime.from(Converter.DATE_TIME_FORMATTER.parse(str)).toOffsetDateTime();
    }

    private static final DateTimeFormatter TIME_FORMATTER = new DateTimeFormatterBuilder()
            .appendOptional(DateTimeFormatter.ISO_TIME)
            .appendOptional(DateTimeFormatter.ISO_OFFSET_TIME)
            .parseDefaulting(ChronoField.YEAR, 2020)
            .parseDefaulting(ChronoField.MONTH_OF_YEAR, 1)
            .parseDefaulting(ChronoField.DAY_OF_MONTH, 1)
            .toFormatter()
            .withZone(ZoneOffset.UTC);

    public static OffsetTime parseTimeString(String str) {
        return ZonedDateTime.from(Converter.TIME_FORMATTER.parse(str)).toOffsetDateTime().toOffsetTime();
    }
    // Serialize/deserialize helpers

    public static FortuneTellerData fromJsonString(String json) throws IOException {
        return getObjectReader().readValue(json);
    }

    public static String toJsonString(FortuneTellerData obj) throws JsonProcessingException {
        return getObjectWriter().writeValueAsString(obj);
    }

    private static ObjectReader reader;
    private static ObjectWriter writer;

    private static void instantiateMapper() {
        ObjectMapper mapper = new ObjectMapper();
        mapper.findAndRegisterModules();
        mapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
        mapper.configure(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS, false);
        SimpleModule module = new SimpleModule();
        module.addDeserializer(OffsetDateTime.class, new JsonDeserializer<OffsetDateTime>() {
            @Override
            public OffsetDateTime deserialize(JsonParser jsonParser, DeserializationContext deserializationContext) throws IOException, JsonProcessingException {
                String value = jsonParser.getText();
                return Converter.parseDateTimeString(value);
            }
        });
        mapper.registerModule(module);
        reader = mapper.readerFor(FortuneTellerData.class);
        writer = mapper.writerFor(FortuneTellerData.class);
    }

    private static ObjectReader getObjectReader() {
        if (reader == null) instantiateMapper();
        return reader;
    }

    private static ObjectWriter getObjectWriter() {
        if (writer == null) instantiateMapper();
        return writer;
    }
}

// FortuneTellerData.java

package com.apiverve.fortuneteller.data;

import com.fasterxml.jackson.annotation.*;
import java.time.OffsetDateTime;

public class FortuneTellerData {
    private String fortune;
    private String insight;
    private String timeframe;
    private String category;
    private Object question;
    private long[] luckyNumbers;
    private String luckyElement;
    private String luckyColor;
    private String luckyDay;
    private OffsetDateTime timestamp;

    @JsonProperty("fortune")
    public String getFortune() { return fortune; }
    @JsonProperty("fortune")
    public void setFortune(String value) { this.fortune = value; }

    @JsonProperty("insight")
    public String getInsight() { return insight; }
    @JsonProperty("insight")
    public void setInsight(String value) { this.insight = value; }

    @JsonProperty("timeframe")
    public String getTimeframe() { return timeframe; }
    @JsonProperty("timeframe")
    public void setTimeframe(String value) { this.timeframe = value; }

    @JsonProperty("category")
    public String getCategory() { return category; }
    @JsonProperty("category")
    public void setCategory(String value) { this.category = value; }

    @JsonProperty("question")
    public Object getQuestion() { return question; }
    @JsonProperty("question")
    public void setQuestion(Object value) { this.question = value; }

    @JsonProperty("luckyNumbers")
    public long[] getLuckyNumbers() { return luckyNumbers; }
    @JsonProperty("luckyNumbers")
    public void setLuckyNumbers(long[] value) { this.luckyNumbers = value; }

    @JsonProperty("luckyElement")
    public String getLuckyElement() { return luckyElement; }
    @JsonProperty("luckyElement")
    public void setLuckyElement(String value) { this.luckyElement = value; }

    @JsonProperty("luckyColor")
    public String getLuckyColor() { return luckyColor; }
    @JsonProperty("luckyColor")
    public void setLuckyColor(String value) { this.luckyColor = value; }

    @JsonProperty("luckyDay")
    public String getLuckyDay() { return luckyDay; }
    @JsonProperty("luckyDay")
    public void setLuckyDay(String value) { this.luckyDay = value; }

    @JsonProperty("timestamp")
    public OffsetDateTime getTimestamp() { return timestamp; }
    @JsonProperty("timestamp")
    public void setTimestamp(OffsetDateTime value) { this.timestamp = value; }
}