package com.realt.statsapi.controllers;

import com.realt.statsapi.controllers.dto.*;
import com.realt.statsapi.data.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.Collection;

@Controller
@RequestMapping("stats")
@CrossOrigin(originPatterns = "*")
public class StatsController {

    private static final Logger log = LoggerFactory.getLogger(StatsController.class);

    @Autowired
    private StatsCountRepository statsCountRepository;
    @Autowired
    private StatsCountYearRepository statsCountYearRepository;

    @Autowired
    private StatsPriceRepository statsPriceRepository;
    @Autowired
    private StatsPriceYearRepository statsPriceYearRepository;

    @Autowired
    private StatsDetailsRepository statsDetailsRepository;

    @GetMapping("/daily/count-legacy")
    public @ResponseBody
    StatsCountResponse getDailyCount() {
        var filter = new StatsFilter();
        return getStatsCount(filter);
    }

    @GetMapping("/daily/count")
    public @ResponseBody
    StatsCountResponse getDailyCountV2(StatsFilter filter) {
        return getStatsCount(filter);
    }

    private StatsCountResponse getStatsCount(StatsFilter filter) {
        log.info("Loading daily count stats {}", filter);
        Collection<DailyCountItem> data = statsCountRepository.find(
                filter.getSource(),
                filter.getStartDate(),
                filter.getEndDate()
        );
        log.info("Loaded daily count stats: {}", data.size());

        return new StatsCountResponse(data.toArray(new DailyCountItem[0]));
    }

    @GetMapping("/daily/count-year")
    public @ResponseBody StatsCountYearResponse getDailyCountYear(StatsFilter filter) {
        log.info("Loading daily count stats (year) {}", filter);
        Collection<DailyCountYearItem> data = statsCountYearRepository.find(
                filter.getSource(),
                filter.getStartDate(),
                filter.getEndDate()
        );
        log.info("Loaded daily count stats (year): {}", data.size());

        return new StatsCountYearResponse(data.toArray(new DailyCountYearItem[0]));
    }


    @GetMapping("/daily/price-legacy")
    public @ResponseBody StatsPriceResponse getDailyPrice() {
        var filter = new StatsFilter();
        return getStatsPrice(filter);
    }

    @GetMapping("/daily/price")
    public @ResponseBody StatsPriceResponse getDailyPriceV2(StatsFilter filter) {
        return getStatsPrice(filter);
    }

    private StatsPriceResponse getStatsPrice(StatsFilter filter) {
        log.info("Loading daily price stats {}", filter);
        Collection<DailyPriceItem> data = statsPriceRepository.find(
                filter.getSource(),
                filter.getStartDate(),
                filter.getEndDate()
        );
        log.info("Loaded daily price stats: {}", data.size());

        return new StatsPriceResponse(data.toArray(new DailyPriceItem[0]));
    }

    @GetMapping("/daily/price-year")
    public @ResponseBody StatsPriceYearResponse getDailyPriceYear(StatsFilter filter) {
        log.info("Loading daily price stats (year) {}", filter);
        Collection<DailyPriceYearItem> data = statsPriceYearRepository.find(
                filter.getSource(),
                filter.getStartDate(),
                filter.getEndDate()
        );
        log.info("Loaded daily price stats (year): {}", data.size());

        return new StatsPriceYearResponse(data.toArray(new DailyPriceYearItem[0]));
    }

    @GetMapping("/daily/detailed")
    public @ResponseBody StatsDetailsResponse getDailyDetails() {

        log.debug("Loading detailed daily stats");
        Collection<DailyDetails> data = statsDetailsRepository.findAll();
        log.info("Loaded detailed daily stats: {}", data.size());

        return new StatsDetailsResponse(data.toArray(new DailyDetails[0]));
    }

    @GetMapping("/daily/detailed/{day}")
    public @ResponseBody StatsDetailsResponse getDailyDetails(@PathVariable String day) {

        log.debug("Loading daily stats for {}", day);
//        DailyDetails details = new DailyDetails();
//        details.setDay(date);
//        Example<DailyDetails> example = Example.of(details);
//        Collection<DailyDetails> data = statsDetailsRepository.findAll(example);
        Collection<DailyDetails> data = statsDetailsRepository.findByDay(day);
        if (data.isEmpty()){
            log.warn("Not found for {}", day);
            throw new ResponseStatusException(
                    HttpStatus.NOT_FOUND, "Daily Stats Not Found");
        }

        log.info("Loaded daily details for {}: {}", day, data.size());

        return new StatsDetailsResponse(data.toArray(new DailyDetails[0]));
    }
}